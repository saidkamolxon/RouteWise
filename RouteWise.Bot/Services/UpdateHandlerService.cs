using RouteWise.Bot.Constants;
using RouteWise.Bot.Constants.Keyboard;
using RouteWise.Bot.Constants.Message;
using RouteWise.Bot.Extensions;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.States;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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
        IStateMachine stateMachine)
    {
        _logger = logger;
        _botClient = botClient;
        _userService = userService;
        _stateMachine = stateMachine;
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

        switch (callbackQuery.Data)
        {
            case "request_an_access":
                await _botClient.AnswerCallbackQueryAsync(new AnswerCallbackQueryRequest { CallbackQueryId = callbackQuery.Id, Text = "Request has been sent" });
                await _botClient.SendMessageAsync(TeplateMessages.RequestAccessMessage(callbackQuery.From));
                break;

            default:
                break;
        }

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
            await _botClient.SendMessageAsync(new SendMessageRequest
            {
                ChatId = message.Chat.Id,
                Text = "You can't use the bot. Firstly, request an access from the admin 👇",
                ReplyMarkup = InlineKeyboards.RequestKeyboard
            });

            return;
        }

        if (message.Text.StartsWith(BotCommands.Start))
        {
            await _stateMachine.SetState(new Models.StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id}, new InitialState(_stateMachine));
        }

        var result = await _stateMachine.FireEvent(message);
        await _botClient.SendMessageAsync(new SendMessageRequest
            {
                ChatId = message.Chat.Id,
                Text = result.AnswerMessage,
                ParseMode = ParseMode.Html,
                ReplyParameters = new ReplyParameters { MessageId = message.MessageId }
            });

        _logger.LogInformation($"Message received: {message.Type}");
    }
}
