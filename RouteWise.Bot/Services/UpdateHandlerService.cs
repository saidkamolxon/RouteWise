using RouteWise.Bot.Constants;
using RouteWise.Bot.Enums;
using RouteWise.Service.Interfaces;
using Stateless;
using System.Runtime.CompilerServices;
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

        _stateMachine = new StateMachine<BotState, BotTrigger>(BotState.InitialState);

        _stateMachine.Configure(BotState.InitialState)
            .Permit(BotTrigger.MeasureDistance, BotState.WaitingForOrigin);

        _stateMachine.Configure(BotState.WaitingForOrigin)
            .Permit(BotTrigger.OriginReceived, BotState.WaitingForDestination);

        _stateMachine.Configure(BotState.WaitingForDestination)
            .Permit(BotTrigger.DestinationReceived, BotState.InitialState);

    }

    public async Task HandleUpdateAsync(Update update)
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
        catch (Exception ex)
        {
            await HandlerErrorAsync(ex);
        }
    }

    private async Task BotOnTextMessageReceived(Message message)
    {
        string messageText = message.Text.Split()[0];
        if (!BotCommands.Contains(messageText))
        {
            switch (_stateMachine.State)
            {
                case BotState.InitialState:
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Originsss");
                    _stateMachine.Fire(BotTrigger.MeasureDistance);
                    break;

                case BotState.WaitingForOrigin:
                    _origin = messageText;
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Destination");
                    _stateMachine.Fire(BotTrigger.OriginReceived);
                    break;

                case BotState.WaitingForDestination:
                    _destination = messageText;
                    string distance = await _googleMapsService.GetDistanceAsync(_origin, _destination);
                    await _botClient.SendTextMessageAsync(message.Chat.Id, distance);
                    _stateMachine.Fire(BotTrigger.DestinationReceived);
                    break;
            };
        }
        else
        {
            switch(messageText)
            {
                case BotCommands.MeasureDistance:
                    _stateMachine.Fire(BotTrigger.MeasureDistance);
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Origin");
                    break;
                default:
                    break;
            }
        }
    }
}
