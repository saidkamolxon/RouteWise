using MassTransit.Internals;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RouteWise.Bot.Controllers;

public class SamsaraWebhookController(ILogger<SamsaraWebhookController> logger) : ControllerBase
{
    private readonly ILogger<SamsaraWebhookController> logger = logger;

    [HttpPost]
    public IActionResult Post([FromBody] JsonElement notification)
    {
        Console.WriteLine(notification.GetProperty("event").GetProperty("text").GetString());
        switch (notification.GetProperty("eventType").GetString())
        {
            case "Alert":
                switch (notification.GetProperty("event").GetProperty("alertConditionId").GetString())
                {
                    case "DeviceLocationInsideGeofence":
                        this.logger.LogWarning("This device is inside geofence.");
                        break;

                    case "DeviceSpeedAboveSpeedLimit":
                        break;
                }
                break;
            case "Ping":
                this.logger.LogInformation("Webhook has been set up successfully. --->>>");
                break;
        }

        return Ok();
    }
}
