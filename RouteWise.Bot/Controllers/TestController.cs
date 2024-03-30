using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

public class TestController : BaseController
{
    private readonly ITrailerService _service;

    public TestController(ITrailerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoadReadyTrailers()
    {
        await _service.UpdateTrailersStatesAsync();
        return Ok();
    }

    [HttpGet("Trailers")]
    public async Task<IActionResult> GetAllTrailers()
    {
        return Ok(await _service.GetAllAsync());
    }
}
