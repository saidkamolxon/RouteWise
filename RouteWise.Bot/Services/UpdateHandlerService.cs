using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Services;

public class UpdateHandlerService
{
    private ILogger<ConfigureWebhook> _logger;
    private ITelegramBotClient _botClient;

    public UpdateHandlerService(ILogger<ConfigureWebhook> logger, ITelegramBotClient botClient)
    {
        _logger = logger;
        _botClient = botClient;
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
            MessageType.Text => _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: message.Text,
                replyToMessageId: message.MessageId),

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
}
