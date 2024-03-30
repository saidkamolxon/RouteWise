using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

public class TestController : BaseController
{
    private readonly ITrailerService _trailerService;
    private readonly ILandmarkService _landmarkService;

    public TestController(ITrailerService trailerService, ILandmarkService landmarkService)
    {
        _trailerService = trailerService;
        _landmarkService = landmarkService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoadReadyTrailers()
    {
        await _trailerService.UpdateTrailersStatesAsync();
        return Ok();
    }

    [HttpGet("Landmarks")]
    public async Task<IActionResult> GetLandmarks()
    {
        return Ok(await _landmarkService.GetAllLandmarksAsync());
    }
    
    [HttpGet("Landmarks/{name}")]
    public async Task<IActionResult> GetLandmarks(string name)
    {
        return Ok(await _landmarkService.GetLandmarksByNameAsync(name));
    }

    [HttpGet("UpdateLandmarks")]
    public async Task<ActionResult> UpdateLandmarks()
    {
        await _landmarkService.UpdateLandmarksAsync();
        return Ok();
    }

    [HttpGet("Trailers")]
    public async Task<IActionResult> GetAllTrailers()
    {
        return Ok(await _trailerService.GetAllAsync());
    }
}
