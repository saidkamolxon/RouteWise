using RouteWise.Bot.Constants;
using RouteWise.Bot.Extensions;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Service.Helpers;
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

        var command = message.GetBotCommand();

        if (command == null) return message.GetHtmlText(); 

        switch (command)
        {
            case BotCommands.Start:
                return $"Assalomu alaykum. {HtmlDecoration.BoldItalic(message.From.FirstName)}.";

            case BotCommands.MeasureDistance:
                await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new DistanceOriginState(_stateMachine));
                return "Enter the origin";

            default: return "Unknown command";
        }
    }
}