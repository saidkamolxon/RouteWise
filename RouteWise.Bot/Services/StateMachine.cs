using Microsoft.EntityFrameworkCore;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Data.Contexts;
using Newtonsoft.Json;
using RouteWise.Domain.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RouteWise.Bot.Services;

public class StateMachine : IStateMachine
{
    private readonly Func<IState> _initialStateFactory;
    private readonly AppDbContext _appDbContext;

    public StateMachine(Func<IState> initialStateFactory, AppDbContext appDbContext)
    {
        _initialStateFactory = initialStateFactory;
        _appDbContext = appDbContext;
    }

    public async Task<MessageEventResult> FireEvent(MessageEvent data)
    {
        var stateEntity = await _appDbContext.States.AsNoTracking().FirstOrDefaultAsync(s => s.ChatId.Equals(data.ChatId));

        if (stateEntity is not null)
        {
            var currentState = DeserializeState(stateEntity.SerializedState);
            return await currentState.Update(data);
        }

        var state = _initialStateFactory();
        await SetState(data.ChatId, state);
        return await state.Update(data);
    }

    public async Task SetState(long chatId, IState nextState)
    {
        var currentState = await _appDbContext.States.FirstOrDefaultAsync(s => s.ChatId.Equals(chatId));

        if (currentState is not null)
            currentState.SerializedState = SerializeState(nextState);
        else
            await _appDbContext.States.AddAsync(new State { ChatId = chatId, SerializedState = SerializeState(nextState) });

        await _appDbContext.SaveChangesAsync();
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