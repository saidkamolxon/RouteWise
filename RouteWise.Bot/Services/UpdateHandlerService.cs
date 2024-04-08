using RouteWise.Bot.Enums;
using RouteWise.Service.Interfaces;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Services;

public class UpdateHandlerService
{
    private ILogger<ConfigureWebhook> _logger;
    private ITelegramBotClient _botClient;
    private StateMachine<BotState, BotTrigger> _stateMachine;
    private ITrailerService _trailerService;
    private readonly IGoogleMapsService _googleMapsService;
    private string _origin;
    private string _destination;

    public UpdateHandlerService(ILogger<ConfigureWebhook> logger,
        ITelegramBotClient botClient,
        ITrailerService trailerService,
        IGoogleMapsService googleMapsService)
    {
        _logger = logger;
        _botClient = botClient;
        _trailerService = trailerService;
        _googleMapsService = googleMapsService;
        _stateMachine = new StateMachine<BotState, BotTrigger>(BotState.Finished);

        _stateMachine.Configure(BotState.Start)
            .Permit(BotTrigger.InputReceived, BotState.WaitingForOrigin);

        _stateMachine.Configure(BotState.WaitingForOrigin)
            .Permit(BotTrigger.OriginReceived, BotState.WaitingForDestination);

        _stateMachine.Configure(BotState.WaitingForDestination)
            .Permit(BotTrigger.DestinationReceived, BotState.Finished);

    }

    public async Task EchoAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message),
            UpdateType.CallbackQuery => BotOnCallBackQueryReceived(update.CallbackQuery),
            _ => UnknownUpdateTypeHandler(update)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            await HandlerErrorAsync(ex);
        }
    }

    private Task HandlerErrorAsync(Exception ex)
    {
        var ErrorMessage = ex switch
        {
            ApiRequestException exception => $"Telegram API Error:\n{exception.ErrorCode}",
            _ => ex.ToString(),
        };

        _logger.LogInformation(ErrorMessage);

        return Task.CompletedTask;
    }

    private Task UnknownUpdateTypeHandler(Update update)
    {
        _logger.LogInformation($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }

    private async Task BotOnCallBackQueryReceived(CallbackQuery callbackQuery)
    {
        _logger.LogInformation($"CallbackQuery received: {callbackQuery}");

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: $"CallbackQuery received: {callbackQuery.Data}");
    }

    private async Task BotOnMessageReceived(Message message)
    {
        _logger.LogInformation($"Message received: {message.Type}");

        var reply = message.Type switch
        {
            MessageType.Text => BotOnTextMessageReceived(message),

            _ => _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "I'm not configured for that type, sorry",
                replyToMessageId: message.MessageId)
        };

        try
        {
            await reply;
        }
        catch(Exception ex)
        {
            await HandlerErrorAsync(ex);
        }
    }

    private async Task BotOnTextMessageReceived(Message message)
    {
        string command = message.Text.Split()[0];

        if (command == "/distance")
        {
            while (_stateMachine.State != BotState.Finished)
            {
                await MeasureDistanceAsync(message);
            }
        }

        
        


        
        //if (message.Text.Contains("/g"))
        //{
        //    var trailerName = message.Text.Split()[1].Trim();
        //    var trailer = await _trailerService.GetByNameAsync(trailerName);
        //    await _botClient.SendPhotoAsync(
        //        chatId: message.Chat.Id, 
        //        caption: trailer.ToString(),
        //        photo: InputFile.FromString(trailer.PhotoUrl),
        //        parseMode: ParseMode.Html);
        //}
        //return;
    }

    private async Task MeasureDistanceAsync(Message message)
    {
        switch (_stateMachine.State)
        {
            case BotState.Start:
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Origin");
                _stateMachine.Fire(BotTrigger.InputReceived);
                break;

            case BotState.WaitingForOrigin:
                _origin = message.Text;
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Destination");
                _stateMachine.Fire(BotTrigger.OriginReceived);
                break;

            case BotState.WaitingForDestination:
                _destination = message.Text;
                string distance = await _googleMapsService.GetDistanceAsync(_origin, _destination);
                await _botClient.SendTextMessageAsync(message.Chat.Id, distance);
                _stateMachine.Fire(BotTrigger.DestinationReceived);
                break;
        };
    }
}
