using RouteWise.Bot.Enums;
using RouteWise.Bot.Models;
using RouteWise.Service.Interfaces;
using Telegram.Bot;

namespace RouteWise.Bot.Handlers;

public class NotificationHandler(ITelegramBotClient botClient, ISamsaraService service)
{
    private readonly ITelegramBotClient botClient = botClient;
    private readonly ISamsaraService service = service;

    public async Task HandleAsync(Notification notification)
    {
        var handler = notification.EventType switch
        {
            EventType.Ping => WhenPingNotificationReceivedAsync(notification),
            EventType.GeofenceEntry => WhenGeofenceEntryNotificationReceivedAsync(notification),
            EventType.GeofenceExit => WhenGeofenceExitNotificationReceivedAsync(notification),
            EventType.SevereSpeedingStarted => WhenGeofenceExitNotificationReceivedAsync(notification),
            EventType.AlertIncident
                when notification.Data["configurationId"] == "a838b48a-935f-40fd-8a64-8e8e4f8ed606"
                    => WhenStoppedForHalfHourNotificationReceived(notification),
            
            _ => WhenUnknownNotificationReceivedAsync(notification)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {

        }
    }

    private async Task WhenGeofenceEntryNotificationReceivedAsync(Notification notification)
    {
        var geofence = notification.Data["address"]["name"];
        var vehicle = notification.Data["vehicle"]["name"];

        await this.botClient.SendTextMessageAsync(6877143602, $"Vehicle # {vehicle} has just arrived at {geofence}.");
    }

    private async Task WhenPingNotificationReceivedAsync(Notification notification)
    {
        await this.botClient.SendTextMessageAsync(6877143602, "Webhook tested. Success!");
    }

    private async Task WhenGeofenceExitNotificationReceivedAsync(Notification notification)
    {
        var geofence = notification.Data["address"]["name"];
        var vehicle = notification.Data["vehicle"]["name"];

        await this.botClient.SendTextMessageAsync(6877143602, $"Vehicle # {vehicle} has just left {geofence}.");
    }

    private async Task WhenSpeedingNotificationReceivedAsync(Notification notification)
    {
        //var truck = await this.service.
        throw new NotImplementedException();
    }

    private async Task WhenStoppedForHalfHourNotificationReceived(Notification notification)
    {
        var truck = notification.Data["conditions"]["details"]["speed"]["vehicle"]["name"];
        await this.botClient.SendTextMessageAsync(6877143602, $"Vehicle # {truck} has been stopped for more than 30 minutes.\n\nFor more info please visit:\n{notification.Data["incidentUrl"]}");
    }

    private async Task WhenUnknownNotificationReceivedAsync(Notification notification)
    {
        throw new NotImplementedException();
    }
}
