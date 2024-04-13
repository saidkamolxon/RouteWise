using AutoMapper;
using Newtonsoft.Json;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Services;

public class StateMachine : IStateMachine
{
    private readonly Func<IState> _initialStateFactory;
    private readonly IUnitOfWork _unitOfWork;

    public StateMachine(Func<IState> initialStateFactory, IUnitOfWork unitOfWork)
    {
        _initialStateFactory = initialStateFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<MessageEventResult> FireEvent(Message message)
    {
        var stateEntity = await _unitOfWork.StateRepository
            .SelectAsync(s => 
                s.ChatId.Equals(message.Chat.Id) &&
                s.UserId.Equals(message.From.Id),
            asNoTracking: true);

        if (stateEntity is not null)
        {
            var currentState = DeserializeState(stateEntity.SerializedState);
            return await currentState.Update(message);
        }

        var state = _initialStateFactory();
        await SetState(new StateValuesDto
        {
            ChatId = message.Chat.Id,
            UserId = message.From.Id,
        }, state);
        return await state.Update(message);
    }

    public async Task SetState(StateValuesDto dto, IState nextState)
    {
        var currentState = await _unitOfWork.StateRepository
            .SelectAsync(s => s.ChatId.Equals(dto.ChatId) && s.UserId.Equals(dto.UserId));

        if (currentState is not null)
        {
            currentState.SerializedState = SerializeState(nextState);
            currentState.DistanceOrigin = dto.DistanceOrigin ?? currentState.DistanceOrigin;
            currentState.DistanceDestination = dto.DistanceDestination ?? currentState.DistanceDestination;
        }
        else
            await _unitOfWork.StateRepository.CreateAsync(new State
            {
                ChatId = dto.ChatId,
                UserId = dto.UserId,
                DistanceOrigin = dto.DistanceOrigin,
                DistanceDestination = dto.DistanceDestination,
                SerializedState = SerializeState(nextState)
            });

        await _unitOfWork.SaveAsync();
    }

    private string SerializeState(IState state)
    {
        return JsonConvert.SerializeObject(state, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
    }

    private IState DeserializeState(string serializedState)
    {
        var obj = JsonConvert.DeserializeObject(serializedState, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        var state = obj as IState
            ?? throw new InvalidOperationException("Deserialized object is not of type IState.");

        return (IState)Activator.CreateInstance(state.GetType(), this);
    }
}