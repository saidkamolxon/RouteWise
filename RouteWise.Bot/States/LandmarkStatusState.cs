using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Service.Interfaces;
using Telegram.Bot.Types;

namespace RouteWise.Bot.States;

public class LandmarkStatusState : IState
{
    private readonly IStateMachine _stateMachine;

    public LandmarkStatusState(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task<MessageEventResult> Update(Message message)
    {
        await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new InitialState(_stateMachine));

        using (var scope = _stateMachine.ServiceProvider.CreateScope())
        {
            var landmarkService = scope.ServiceProvider.GetRequiredService<ILandmarkService>();

            var landmark = await landmarkService.GetLandmarksByNameAsync(message.Text);

            return landmark.FirstOrDefault().Address.ToString() ?? "Error";
        }
    }
}
