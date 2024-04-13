using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;

namespace RouteWise.Bot.States;

public class DistanceOriginState : IState
{
    private readonly IStateMachine _stateMachine;

    public DistanceOriginState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(MessageEvent data)
    {
        await _stateMachine.SetState(data.ChatId, new DistanceDestinationState(_stateMachine));

        return "Enter the destination";
    }
}