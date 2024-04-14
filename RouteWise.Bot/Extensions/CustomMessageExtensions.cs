using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Extensions;

public static class CustomMessageExtensions
{
    public static string GetBotCommand(this Message message)
    {
        if (message?.Entities is null) return null;

        if (message.Entities.First().Type is MessageEntityType.BotCommand)
            return message.EntityValues.First().Split("@").First();
        
        return null;
    }
}
