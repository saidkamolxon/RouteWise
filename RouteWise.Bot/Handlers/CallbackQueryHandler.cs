using RouteWise.Bot.Constants.Message;
using RouteWise.Service.DTOs.User;
using RouteWise.Service.Helpers;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using RouteWise.Bot.Extensions;

namespace RouteWise.Bot.Services;

public partial class UpdateHandlerService
{
    private async Task BotOnCallBackQueryReceived(CallbackQuery callbackQuery)
    {
        _logger.LogInformation($"CallbackQuery received: {callbackQuery}");

        var message = callbackQuery.GetMessage();

        switch (callbackQuery.Data)
        {
            case "request_an_access":
                await _botClient.AnswerCallbackQueryAsync(new AnswerCallbackQueryRequest { CallbackQueryId = callbackQuery.Id, Text = "Request has been sent" });
                await _botClient.SendMessageAsync(TemplateMessages.RequestAccessMessage(user: callbackQuery.From));
                await _botClient.EditMessageTextAsync(new EditMessageTextRequest { ChatId = message.Chat.Id, MessageId = message.MessageId, Text = "Request has been sent." });
                break;

            case "accept_the_request":
                var data = message.Text.Split();
                await _userService.AddAsync(new UserCreationDto
                {
                    FirstName = data[0],
                    LastName = data[1],
                    TelegramId = long.Parse(data[2])
                });
                await _botClient.EditMessageTextAsync(new EditMessageTextRequest { ChatId = message.Chat.Id, MessageId = message.MessageId, Text = HtmlDecoration.Bold("Accepted ✅"), ParseMode = ParseMode.Html });
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
}
