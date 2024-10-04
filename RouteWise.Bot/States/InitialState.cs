using RouteWise.Bot.Constants;
using RouteWise.Bot.Extensions;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Domain.Enums;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        var command = message.GetBotCommand();
        var commandArgs = message.GetBotCommandArgs();

        if (command == null)
            return message.GetHtmlText();

        switch (command)
        {
            case BotCommands.Start:
                return $"Assalomu alaykum. {HtmlDecoration.Bold(message.From.GetFullName())}.";

            case BotCommands.MeasureDistance:
                await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new DistanceOriginState(_stateMachine));
                return "Enter the origin";

            case BotCommands.GetLandmarkStatus:
                await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new LandmarkStatusState(_stateMachine));
                return "Enter the name of the lane:";

            case BotCommands.GetTrailerStatus:
                using (var scope = _stateMachine.ServiceProvider.CreateScope())
                {
                    var trailerService = scope.ServiceProvider.GetRequiredService<ITrailerService>();
                    var trailer = await trailerService.GetByNameAsync(commandArgs.First());
                    var result = new MessageEventResult
                    {
                        Type = MessageType.Photo,
                        Text = trailer.ToString(),
                        File = new PhotoSize { FileId = trailer.PhotoUrl }
                    };
                    return result;
                }

            case BotCommands.GetTruckList:
                using (var scope = _stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDitatTmsService>();
                    var result = await service.GetAvailableTrucksAsync();
                    return result;
                }

            case BotCommands.GetTruckListWithoutDrivers:
                using (var scope = _stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDitatTmsService>();
                    return await service.GetAvailableTrucksAsync(withDrivers: false);
                }

            case BotCommands.GetUnitDocuments:
                using (var scope = _stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDitatTmsService>();
                    var docs = await service.GetUnitDocumentsAsync(commandArgs.First(), UnitType.Trailer);
                    var docUrls = docs.Select(d => new Document { FileId = d.ToString() });

                    var result = new MessageEventResult
                    {
                        Type = MessageType.Document,
                        IsMediaGroup = true,
                        Files = [..docUrls.Cast<FileBase>()]
                    };
                    return result;
                }

            case BotCommands.GetEtaToDestination:
                using (var scope = _stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ITruckService>();
                    var origin = (await service.GetByNameAsync(commandArgs.First())).Coordinates;
                    await _stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id, DistanceOrigin = origin.ToString() }, new DistanceDestinationState(_stateMachine));
                    return "Enter the destination";
                }

            case BotCommands.GetAllTrailersInfo:
                //TODO need to implement code for this case
                return "This is gonna be all trailer info";

            default: return "Unknown command";
        }
    }
}