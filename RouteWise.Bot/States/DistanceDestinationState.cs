using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Data.IRepositories;
using RouteWise.Service.Interfaces;
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
        
        using (var scope = _stateMachine.ServiceProvider.CreateScope())
        {
            var googleMapsService = scope.ServiceProvider.GetRequiredService<IGoogleMapsService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var stateValues = await unitOfWork.StateRepository.SelectAsync(s => s.ChatId.Equals(message.Chat.Id) && s.UserId.Equals(message.From.Id));

            return await googleMapsService.GetDistanceAsync(stateValues.DistanceOrigin, stateValues.DistanceDestination);
        }
    }
}