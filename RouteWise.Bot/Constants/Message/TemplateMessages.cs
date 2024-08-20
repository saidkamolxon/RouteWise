using RouteWise.Bot.Extensions;
using RouteWise.Service.Helpers;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Constants.Message;

public static class TemplateMessages
{
    public static string RequestAccessMessage(User user)
        => $"{HtmlDecoration.Bold(user.GetFullName())}\n" +
           $"{HtmlDecoration.Code(user.Id.ToString())}\n" +
           "The user is asking a permission to use the bot";
}
