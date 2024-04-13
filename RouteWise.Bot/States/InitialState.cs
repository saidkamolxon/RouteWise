using RouteWise.Bot.Constants;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;

namespace RouteWise.Bot.States;

public class InitialState : IState
{
    private readonly IStateMachine _stateMachine;

    public InitialState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(MessageEvent data)
    {
        if (data?.Message?.Text is null)
            return "There is nothing I can do for you";

        if (!BotCommands.Contains(data.Message.Text))
        {
            //TODO need to implement sending message to chats method
            return data.Message.Text; // return the message itself
        }

        if (data.Message.Text.StartsWith(BotCommands.MeasureDistance))
        {
            var distanceOriginState = new DistanceOriginState(_stateMachine);
            await _stateMachine.SetState(data.ChatId, distanceOriginState);
            return "Enter the origin";
        }

        return "Command is not found.";
    }
}