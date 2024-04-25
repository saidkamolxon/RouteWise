using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Domain.Entities;
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

            var landmarks = await landmarkService.GetLandmarksByNameAsync(message.Text);
            
            foreach (var landmark in landmarks)
            {
                var trailersCoordinates = landmark.Trailers.Select(x => x.Coordinates.ToString()).ToArray();
                return new MessageEventResult { AnswerMessage = landmark.ToString(), PhotoUrl = landmark.PhotoUrl };
            }
            
            return landmarks.FirstOrDefault().ToString();
        }
    }
}
