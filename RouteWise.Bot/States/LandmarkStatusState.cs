using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Service.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

            var result = new MessageEventResult { Type = MessageType.Photo };

            foreach (var landmark in landmarks)
            {
                var trailersCoordinates = landmark.Trailers.Select(x => x.Coordinates.ToString()).ToArray();
                result.Texts.Add(landmark.ToString());
                result.Files.Add(new PhotoSize { FileId = landmark.PhotoUrl });
            }

            return result;
        }
    }
}
