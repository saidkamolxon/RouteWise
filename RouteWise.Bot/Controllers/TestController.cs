using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

public class TestController : BaseController
{
    private readonly IRoadReadyService _roadReadyService;

    public TestController(IRoadReadyService roadReadyService)
    {
        _roadReadyService = roadReadyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoadReadyTrailers()
    {
        var result = await _roadReadyService.GetTrailersStatesAsync();
        return Ok(result);
    }
}
