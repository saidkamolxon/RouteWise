using RouteWise.Bot.Constants.Keyboard;
using RouteWise.Bot.Constants.Message;
using RouteWise.Bot.Extensions;
using RouteWise.Service.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Handlers;

public partial class UpdateHandler
{
    private async Task BotOnCallBackQueryReceived(CallbackQuery callbackQuery)
    {
        logger.LogInformation("CallbackQuery received: {callbackQuery}", callbackQuery);
        
        var message = callbackQuery.GetMessage();

        switch (callbackQuery.Data)
        {
            case "request_an_access":
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Request has been sent");
                await botClient.SendTextMessageAsync(6877143602, TemplateMessages.RequestAccessMessage(callbackQuery.From), replyMarkup: InlineKeyboards.ResponseKeyboard, parseMode: ParseMode.Html);
                await botClient.EditMessageTextOrCaptionAsync(message, "Request has been sent.");
                break;

            case "accept_the_request":
                var data = message.Text.Split('\n');
                var newUser = await userService.AddAsync(new()
                {
                    FirstName = data[0],
                    //LastName = data[1],
                    TelegramId = long.Parse(data[1])
                });
                await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, $"{message.GetHtmlText()}\n\n{HtmlDecoration.Bold("Accepted ✅")}", parseMode: ParseMode.Html);
                await botClient.SendTextMessageAsync(newUser.TelegramId, $"✅ Your request has been accepted. Your current role is {newUser.Role}");
                break;

            case "reject_the_request":
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Rejected");
                await botClient.EditMessageTextOrCaptionAsync(message, $"{message.GetHtmlText()}\n\n{HtmlDecoration.Bold("Rejected ❌")}", parseMode: ParseMode.Html);
                await botClient.SendTextMessageAsync(
                    chatId: message.Text.Split('\n').ElementAt(1),
                    text: HtmlDecoration.Bold("❌ Sorry but your request has been rejected by the owner."),
                    parseMode: ParseMode.Html
                );
                break;

            case "cancel":
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "❌ Canceled");
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                break;

            default:
                break;
        }
    }
}
