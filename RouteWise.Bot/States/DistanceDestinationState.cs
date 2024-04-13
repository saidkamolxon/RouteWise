using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using Telegram.Bot.Types;

namespace RouteWise.Bot.States;

public class DistanceDestinationState : IState
{
    private readonly IStateMachine _stateMachine;

    public DistanceDestinationState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(Message message)
    {
        await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id, DistanceDestination = message.Text }, new InitialState(_stateMachine));
        return "Distance is about 1000 miles";
    }
}