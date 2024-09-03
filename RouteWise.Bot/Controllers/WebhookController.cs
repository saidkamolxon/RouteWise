using Microsoft.AspNetCore.Mvc;
using RouteWise.Bot.Handlers;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Controllers;

public class WebhookController(ILogger<WebhookController> logger) : ControllerBase
{
    private readonly ILogger<WebhookController> logger = logger;

    [HttpPost]
    public async Task<IActionResult> Post([FromServices] UpdateHandler handler, [FromBody] Update update)
    {
        await handler.HandleUpdateAsync(update);
        return Ok();
    }

    //[HttpPost]
    //public IActionResult HandleAlertFromSamsara([FromBody] dynamic notification)
    //{
    //    if (notification == null)
    //    {
    //        this.logger.LogWarning("Received an empty notification");
    //        return BadRequest();
    //    }

    //    try
    //    {
    //        string alertConditionId = notification["event"]["alertConditionId"];
    //        switch (alertConditionId)
    //        {
    //            case "DeviceLocationInsideGeofence":
    //                this.logger.LogInformation("Geofence entry alert received.");
    //                break;
    //            case "DeviceSpeedAboveSpeedLimit":
    //                this.logger.LogInformation("Speeding alert received.");
    //                break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        this.logger.LogError(ex, "Error processing the webhook notification.");
    //        return BadRequest();
    //    }

    //    return Ok();
    //}
}
