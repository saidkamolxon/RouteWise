using RouteWise.Bot.Models;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Interfaces;

public interface IStateMachine
{
    public IServiceProvider ServiceProvider { get; }
    Task<MessageEventResult> FireEvent(Message message);
    Task SetState(StateValuesDto dto, IState nextState);
}