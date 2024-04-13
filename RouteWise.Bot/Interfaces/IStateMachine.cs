using RouteWise.Bot.Models;

namespace RouteWise.Bot.Interfaces;

public interface IStateMachine
{
    Task<MessageEventResult> FireEvent(MessageEvent data);
    Task SetState(long chatId, IState nextState);
}