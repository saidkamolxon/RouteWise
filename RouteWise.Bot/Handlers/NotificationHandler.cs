using RouteWise.Bot.Enums;
using RouteWise.Bot.Models;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Handlers;

public class NotificationHandler(ILogger<NotificationHandler> logger, ITelegramBotClient botClient, ISamsaraService service)
{
    private readonly ILogger<NotificationHandler> logger = logger;
    private readonly ITelegramBotClient botClient = botClient;
    private readonly ISamsaraService service = service;
    private readonly ChatId chatId = -4581289901;

    public async Task HandleAsync(Notification notification)
    {
        var handler = notification.EventType switch
        {
            EventType.Ping => WhenPingNotificationReceivedAsync(notification),
            EventType.GeofenceEntry => WhenGeofenceEntryNotificationReceivedAsync(notification),
            EventType.GeofenceExit => WhenGeofenceExitNotificationReceivedAsync(notification),
            EventType.SevereSpeedingStarted => WhenGeofenceExitNotificationReceivedAsync(notification),
            EventType.AlertIncident => WhenStoppedForHalfHourNotificationReceived(notification),
            
            _ => WhenUnknownNotificationReceivedAsync(notification)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while trying to handle notification from Samsara");
        }
    }

    private async Task WhenGeofenceEntryNotificationReceivedAsync(Notification notification)
    {
        var geofence = notification.Data.GetProperty("address").GetProperty("name").GetString();
        var vehicle = notification.Data.GetProperty("vehicle").GetProperty("name").GetString();

        await this.botClient.SendTextMessageAsync(chatId, $"Vehicle # {vehicle} has just arrived at {geofence}.");
    }

    private async Task WhenPingNotificationReceivedAsync(Notification notification)
    {
        await this.botClient.SendTextMessageAsync(6877143602, "Webhook tested. Success!");
    }

    private async Task WhenGeofenceExitNotificationReceivedAsync(Notification notification)
    {
        var geofence = notification.Data.GetProperty("address").GetProperty("name").GetString();
        var vehicle = notification.Data.GetProperty("vehicle").GetProperty("name").GetString();

        await this.botClient.SendTextMessageAsync(chatId, $"Vehicle # {vehicle} has just left {geofence}.");
    }

    private async Task WhenSpeedingNotificationReceivedAsync(Notification notification)
    {
        //var truck = await this.service.
        throw new NotImplementedException();
    }

    private async Task WhenStoppedForHalfHourNotificationReceived(Notification notification)
    {
        if (notification.Data.GetProperty("configurationId").GetString() == "a838b48a-935f-40fd-8a64-8e8e4f8ed606")
        {
            string truck = notification.Data.GetProperty("conditions")[0].GetProperty("details").GetProperty("speed").GetProperty("vehicle").GetProperty("name").GetString();
            await this.botClient.SendTextMessageAsync(chatId, $"Vehicle # {truck} has been parked for more than 45 minutes.\n\nFor more info please visit:\n{notification.Data.GetProperty("incidentUrl").GetString()}");
        }
    }

    private async Task WhenUnknownNotificationReceivedAsync(Notification notification)
    {
        throw new NotImplementedException();
    }
}
