using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

public class TestController : BaseController
{
    private readonly ITrailerService _trailerService;
    private readonly ILandmarkService _landmarkService;
    private readonly ITruckService _truckService;

    public TestController(ITrailerService trailerService, ILandmarkService landmarkService,
        ITruckService truckService)
    {
        _trailerService = trailerService;
        _landmarkService = landmarkService;
        _truckService = truckService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoadReadyTrailers()
    {
        await _trailerService.UpdateTrailersStatesAsync();
        return Ok();
    }

    //[HttpGet("landmarks")]
    //public async Task<IActionResult> GetLandmarks()
    //{
    //    return Ok(await _landmarkService.GetAllLandmarksAsync());
    //}

    [HttpGet("landmarks")]
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

    [HttpGet("trucks/{name}")]
    public async Task<IActionResult> GetAllTrucks(string name)
    {
        return Ok(await _truckService.GetAsync(name));
    }
}
