using RouteWise.Bot.Constants.Keyboard;
using RouteWise.Bot.Extensions;
using RouteWise.Service.Helpers;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Constants.Message;

public static class TemplateMessages
{
    public static SendMessageRequest RequestAccessMessage(User user)
    {
        return new SendMessageRequest {
            ChatId = Environment.GetEnvironmentVariable("OwnerId"),
            Text = $"{HtmlDecoration.Bold(user.GetFullName())}\n" +
                   $"{HtmlDecoration.Code(user.Id.ToString())}\n" +
                    "The user is asking a permission to use the bot",
            ParseMode = Defaults.DefaultParseMode,
            ReplyMarkup = InlineKeyboards.ResponseKeyboard
        };
    }
}
