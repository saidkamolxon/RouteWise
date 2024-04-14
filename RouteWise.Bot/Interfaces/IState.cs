using Telegram.Bot.Types;

namespace RouteWise.Bot.Interfaces;

public interface IState
{
    Task<MessageEventResult> Update(Message message);
}