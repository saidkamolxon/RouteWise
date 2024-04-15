using RouteWise.Bot.States;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Enums;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Services;

public class StateHandlerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DistanceOriginState _distanceOriginState;

    public StateHandlerService(IUnitOfWork unitOfWork, DistanceOriginState distanceOriginState)
    {
        _unitOfWork = unitOfWork;
        _distanceOriginState = distanceOriginState;
    }

    public async Task HandleAsync(Message message)
    {
        var state = await _unitOfWork.StateRepository
            .SelectAsync(s => s.ChatId.Equals(message.Chat.Id) &&
                              s.UserId.Equals(message.From.Id));

        //switch (state.Step)
        //{
        //    case Step.Initial: return;
        //    case Step.DistanceOrigin:
        //        await _distanceOriginState.Update(message);
        //        break;
        //}

    }
}
