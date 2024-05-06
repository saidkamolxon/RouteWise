using RouteWise.Bot.Constants.Message;
using RouteWise.Bot.Extensions;
using RouteWise.Service.DTOs.User;
using RouteWise.Service.Helpers;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Services;

public partial class UpdateHandlerService
{
    private async Task BotOnCallBackQueryReceived(CallbackQuery callbackQuery)
    {
        _logger.LogInformation("CallbackQuery received: {callbackQuery}", callbackQuery);

        var message = callbackQuery.GetMessage();

        switch (callbackQuery.Data)
        {
            case "request_an_access":
                await _botClient.AnswerCallbackQueryAsync(new AnswerCallbackQueryRequest { CallbackQueryId = callbackQuery.Id, Text = "Request has been sent" });
                await _botClient.SendMessageAsync(TemplateMessages.RequestAccessMessage(user: callbackQuery.From));
                await _botClient.EditMessageTextOrCaptionAsync(message, "Request has been sent.");
                break;

            case "accept_the_request":
                var data = message.Text.Split();
                await _userService.AddAsync(new UserCreationDto
                {
                    FirstName = data[0],
                    LastName = data[1],
                    TelegramId = long.Parse(data[2])
                });
                await _botClient.EditMessageTextOrCaptionAsync(message, HtmlDecoration.Bold("Accepted ✅"));
                break;

            case "reject_the_request":
                await _botClient.AnswerCallbackQueryAsync(new AnswerCallbackQueryRequest { CallbackQueryId = callbackQuery.Id, Text = "Rejected"});
                await _botClient.EditMessageTextOrCaptionAsync(message, HtmlDecoration.Bold("Rejected ❌"));
                await _botClient.SendMessageAsync(new SendMessageRequest
                {
                    ChatId = message.Text.Split().ElementAt(2),
                    Text = HtmlDecoration.Bold("Sorry but your request has been rejected by the owner.")
                });
                break;

            default:
                break;
        }
    }
}
