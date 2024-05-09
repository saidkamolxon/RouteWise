using Microsoft.AspNetCore.Mvc;
using RouteWise.Bot.Handlers;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] UpdateHandler handler, [FromBody] Update update)
    {
        await handler.HandleUpdateAsync(update);
        return Ok();
    }
}