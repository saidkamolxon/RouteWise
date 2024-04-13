using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using Telegram.Bot.Types;

namespace RouteWise.Bot.States;

public class DistanceOriginState : IState
{
    private readonly IStateMachine _stateMachine;

    public DistanceOriginState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(Message message)
    {
        await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id, DistanceOrigin = message.Text }, new DistanceDestinationState(_stateMachine));

        return "Enter the destination";
    }
}