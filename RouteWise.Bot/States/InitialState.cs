using RouteWise.Bot.Constants;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using Telegram.Bot.Types;

namespace RouteWise.Bot.States;

public class InitialState : IState
{
    private readonly IStateMachine _stateMachine;

    public InitialState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(Message message)
    {
        if (message?.Text is null)
            return "There is nothing I can do for you";

        switch (message.Text)
        {
            case BotCommands.Start:
                await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new InitialState(_stateMachine));
                return "Assalamu alaykum.";

            case BotCommands.MeasureDistance:
                await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new DistanceOriginState(_stateMachine));
                return "Enter the origin";

            default:
                return message.Text;
        }
    }
}