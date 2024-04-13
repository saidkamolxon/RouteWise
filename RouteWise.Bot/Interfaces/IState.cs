using RouteWise.Bot.Models;

namespace RouteWise.Bot.Interfaces;

public interface IState
{
    Task<MessageEventResult> Update(MessageEvent data);
}