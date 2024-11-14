using RouteWise.Bot.Constants;
using RouteWise.Bot.Extensions;
using RouteWise.Bot.Helpers;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Models;
using RouteWise.Domain.Enums;
using RouteWise.Service.Brokers.APIs.DitatTms;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.States;

public class InitialState(IStateMachine stateMachine) : IState
{
    private readonly IStateMachine stateMachine = stateMachine;

    public async Task<MessageEventResult> Update(Message message)
    {
        var command = message.GetBotCommand();
        var commandArgs = message.GetBotCommandArgs();

        if (command is null)
            return message.GetBotCommand();

        switch (command.ToLower())
        {
            case BotCommands.Start:
                return $"Assalomu alaykum. {HtmlDecoration.Bold(message.From.GetFullName())}.";

            case BotCommands.MeasureDistance:
                await this.stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new DistanceOriginState(stateMachine));
                return "Enter the origin";

            case BotCommands.LandmarkStatus:
                await this.stateMachine.SetState(new StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new LandmarkStatusState(stateMachine));
                return "Enter the name of the lane:";

            case BotCommands.TrailerStatusGoogle:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
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

            case BotCommands.TruckList:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDitatTmsApiBroker>();
                    var result = await service.GetAvailableTrucksAsync();
                    return result;
                }

            case BotCommands.TruckListWithoutDrivers:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDitatTmsApiBroker>();
                    return await service.GetAvailableTrucksAsync(withDrivers: false);
                }

            case BotCommands.UnitDocuments:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDitatTmsApiBroker>();
                    var docs = await service.GetUnitDocumentsAsync(commandArgs.First(), UnitType.Trailer);
                    var docUrls = docs.Select(d => new Document { FileId = d.ToString() });

                    var result = new MessageEventResult
                    {
                        Type = MessageType.Document,
                        IsMediaGroup = true,
                        Files = [.. docUrls.Cast<FileBase>()]
                    };
                    return result;
                }

            case BotCommands.EtaToDestination:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ITruckService>();
                    var origin = (await service.GetByNameAsync(commandArgs.First())).Coordinates;
                    await this.stateMachine.SetState(
                        new StateValuesDto(chatId: message.Chat.Id, userId: message.From.Id,
                            distanceOrigin: origin.ToString()), new DistanceDestinationState(stateMachine));
                    return "Enter the destination";
                }

            case BotCommands.AllTrailersInfo:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ITrailerService>();
                    var trailers = await service.GetAllAsync();
                    var builder = new StringBuilder();
                    var now = TimeHelper.ConvertUtcToDefaultTime(DateTime.UtcNow);
                    foreach (var trailer in trailers)
                    {
                        var address = trailer.Landmark?.Split("->").First().Trim() ?? trailer.Address;
                        var symbol = trailer.IsMoving ? "🟢" : "🔴";
                        
                        var upd = now - trailer.LastEventAt;
                        if (upd >= TimeSpan.FromHours(24))
                            symbol = "⚠️";

                        builder.AppendLine($"{symbol} <code>{trailer.Name.PadRight(10)}</code> ➜ {address}");
                    }
                    var result = builder.ToString();
                    var messages = MessageHelper.SplitMessage(result);
                    return new MessageEventResult { Type = MessageType.Text, Texts = messages };
                }
                
            case BotCommands.UpdateLandmarks:
                using (var scope = this.stateMachine.ServiceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ILandmarkService>();
                    await service.UpdateLandmarksAsync();
                    await service.RemoveRedundantLandmarks();
                    return "Landmarks updated";
                }

            default:
                return "Unknown command";
        }
    }
}   