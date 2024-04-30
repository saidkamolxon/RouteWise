using Telegram.Bot.Types;

namespace RouteWise.Bot.Handlers;

public class CommandHandlerService
{
    public void Handle(Message message, CommandHandler commandHandler)
    {
        commandHandler(message);
    }
}
