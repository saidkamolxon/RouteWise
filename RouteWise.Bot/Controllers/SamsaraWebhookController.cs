using Microsoft.AspNetCore.Mvc;
using RouteWise.Bot.Handlers;
using RouteWise.Bot.Models;
using System.Text.Json;

namespace RouteWise.Bot.Controllers;

public class SamsaraWebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] NotificationHandler handler, [FromBody] JsonDocument document)
    {
        var rootElement = document.RootElement;
        var notification = JsonSerializer.Deserialize<Notification>(rootElement.GetRawText());
        await handler.HandleAsync(notification);
        
        return Ok();
    }
}
