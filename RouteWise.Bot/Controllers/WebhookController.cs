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
}
