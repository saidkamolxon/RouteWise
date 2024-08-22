using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

public class TestController : BaseController
{
    private readonly ITrailerService _trailerService;
    private readonly ILandmarkService _landmarkService;
    private readonly ITruckService _truckService;
    private readonly IDitatTmsService _ditatTmsService;

    public TestController(ITrailerService trailerService, ILandmarkService landmarkService,
        ITruckService truckService, IDitatTmsService ditatTmsService)
    {
        _trailerService = trailerService;
        _landmarkService = landmarkService;
        _truckService = truckService;
        _ditatTmsService = ditatTmsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoadReadyTrailers()
    {
        await _trailerService.UpdateTrailersStatesAsync();
        return Ok();
    }

    [HttpGet("trailersf")]
    public async Task<IActionResult> GetLandmarks(string city, string state)
    {
        return Ok(await _trailerService.GetByCityAndStateAsync(city, state));
    }

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

    [HttpGet("trucklist")]
    public async Task<IActionResult> GetAvailableTrucks()
    {
        return Ok(await _ditatTmsService.GetAvailableTrucksAsync());
    }

    [HttpGet("trucklist-driverless")]
    public async Task<IActionResult> GetAvailableTrucksDriverless()
    {
        return Ok(await _ditatTmsService.GetAvailableTrucksAsync(false));
    }
}
