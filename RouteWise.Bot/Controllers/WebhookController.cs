using Microsoft.AspNetCore.Mvc;
using RouteWise.Bot.Services;
using Telegram.Bot.Types;

namespace RouteWise.Bot.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] UpdateHandlerService service,
        [FromBody] Update update)
    {
        await service.HandleUpdateAsync(update);
        
        return Ok();
    }
}