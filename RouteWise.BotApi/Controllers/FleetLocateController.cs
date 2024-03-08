using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.BotApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FleetLocateController : ControllerBase
{
    private readonly IFleetLocateService _service;

    public FleetLocateController(IFleetLocateService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssets()
    {
        var assets = await _service.GetAssetsAsync();
        return Ok(assets);
    }
}
