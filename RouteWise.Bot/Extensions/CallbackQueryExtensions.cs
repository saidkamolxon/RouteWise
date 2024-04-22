using Telegram.Bot.Types;

namespace RouteWise.Bot.Extensions;

public static class CallbackQueryExtensions
{
    /// <summary>
    /// Gets <see cref="Message"/> from <see cref="MaybeInaccessibleMessage"/>
    /// </summary>
    /// <returns><see cref="Message"/></returns>
    public static Message GetMessage(this CallbackQuery query)
    {
        return (Message)query.Message;
    }
}