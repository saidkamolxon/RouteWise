using RouteWise.Bot.Interfaces;
using RouteWise.Bot.States;
using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Services;

public class UpdateHandlerService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IUserService _userService;
    private static IStateMachine _stateMachine;

    public UpdateHandlerService(ILogger<ConfigureWebhook> logger,
        ITelegramBotClient botClient,
        IUserService userService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _botClient = botClient;
        _userService = userService;
        _stateMachine = new StateMachine(CreateInitialState, unitOfWork);
    }

    private static IState CreateInitialState()
        => new InitialState(_stateMachine);

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
            _ => ex.ToString()
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

        await _botClient.AnswerCallbackQueryAsync(new AnswerCallbackQueryRequest()
        {
            CallbackQueryId = callbackQuery.Id,
            Text = "Hey you ... "
        });
    }

    private async Task BotOnMessageReceived(Message message)
    {
        if (!_userService.IsPermittedUser(message.From.Id))
        {
            //TODO Request an access
            return;
        }

        _logger.LogInformation($"Message received: {message.Type}");

        var result = await _stateMachine.FireEvent(message);
        await _botClient.SendMessageAsync(new SendMessageRequest
        {
            ChatId = message.Chat.Id,
            Text = result.AnswerMessage
        });
    }
}
