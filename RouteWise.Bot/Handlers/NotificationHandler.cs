using RouteWise.Bot.Enums;
using RouteWise.Bot.Models;
using RouteWise.Service.Brokers.APIs.Samsara;
using RouteWise.Service.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Handlers;

public class NotificationHandler(ILogger<NotificationHandler> logger, ITelegramBotClient botClient, ISamsaraApiBroker service)
{
    private readonly ILogger<NotificationHandler> logger = logger;
    private readonly ITelegramBotClient botClient = botClient;
    private readonly ISamsaraApiBroker service = service;
    private readonly ChatId chatId = -1002299403534;

    public async Task HandleAsync(Notification notification)
    {
        var handler = notification.EventType switch
        {
            EventType.Ping => WhenPingNotificationReceivedAsync(notification),
            EventType.GeofenceEntry => WhenGeofenceEntryNotificationReceivedAsync(notification),
            EventType.GeofenceExit => WhenGeofenceExitNotificationReceivedAsync(notification),
            EventType.SevereSpeedingStarted => WhenSpeedingNotificationReceivedAsync(notification),
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
        var vehicle = notification.Data.GetProperty("vehicle");
        try
        {
            var driver = await this.service.GetDriverByVehicleIdAsync(vehicle.GetProperty("id").GetString());
            await this.botClient.SendTextMessageAsync(chatId, $"🟢 <b>{vehicle.GetProperty("name").GetString()} {driver}</b> arrived at {HtmlDecoration.Bold(geofence)}.", parseMode: ParseMode.Html);
        }
        catch
        { 
            await this.botClient.SendTextMessageAsync(chatId, $"🟢 {HtmlDecoration.Bold(vehicle.GetProperty("name").GetString())} arrived at {HtmlDecoration.Bold(geofence)}.", parseMode: ParseMode.Html);
        }
    }

    private async Task WhenPingNotificationReceivedAsync(Notification notification)
    {
        await this.botClient.SendTextMessageAsync(6877143602, "Webhook tested. Success!", parseMode: ParseMode.Html);
    }

    private async Task WhenGeofenceExitNotificationReceivedAsync(Notification notification)
    {
        var geofence = notification.Data.GetProperty("address").GetProperty("name").GetString();
        var vehicle = notification.Data.GetProperty("vehicle");
        try
        {
            var driver = await this.service.GetDriverByVehicleIdAsync(vehicle.GetProperty("id").GetString());
            await this.botClient.SendTextMessageAsync(chatId, $"🟠 <b>{vehicle.GetProperty("name").GetString()} {driver}</b> left {HtmlDecoration.Bold(geofence)}.", parseMode: ParseMode.Html);
        }
        catch
        {
            await this.botClient.SendTextMessageAsync(chatId, $"🟠 {HtmlDecoration.Bold(vehicle.GetProperty("name").GetString())} left {HtmlDecoration.Bold(geofence)}.", parseMode: ParseMode.Html);
        }
    }

    private async Task WhenSpeedingNotificationReceivedAsync(Notification notification)
    {
        //var truck = 
        throw new NotImplementedException();
    }

    private async Task WhenStoppedForHalfHourNotificationReceived(Notification notification)
    {
        if (notification.Data.GetProperty("configurationId").GetString() == "a838b48a-935f-40fd-8a64-8e8e4f8ed606")
        {
            var vehicle = notification.Data.GetProperty("conditions")[0].GetProperty("details").GetProperty("speed").GetProperty("vehicle");
            string driver = await this.service.GetDriverByVehicleIdAsync(vehicle.GetProperty("id").GetString());
            await this.botClient.SendTextMessageAsync(chatId, $"🛑 <b>{vehicle.GetProperty("name").GetString()} {driver} has stopped for more than 45 minutes.</b>\n\nFor more info please visit:\n{notification.Data.GetProperty("incidentUrl").GetString()}", parseMode: ParseMode.Html);
        }
    }

    private async Task WhenUnknownNotificationReceivedAsync(Notification notification)
    {
        var alertCondition = notification.Event.GetProperty("alertConditionId").GetString();
        switch (alertCondition)
        {
            case "DeviceSevereSpeedAboveSpeedLimit":
                var vehicle = notification.Event.GetProperty("device").GetProperty("name").GetString();
                string driver = await this.service.GetDriverByTruckNameAsync(vehicle);
                await this.botClient.SendTextMessageAsync(chatId, notification.Event.GetProperty("details").GetString());
                break;
        }
    }
}
