using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;

namespace RouteWise.Bot.States;

public class DistanceDestinationState : IState
{
    private readonly IStateMachine _stateMachine;

    public DistanceDestinationState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(MessageEvent data)
    {
        await _stateMachine.SetState(data.ChatId, new InitialState(_stateMachine));
        return "The distance is about 1000 miles";
    }
}