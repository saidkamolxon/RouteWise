using Microsoft.AspNetCore.Mvc;
using RouteWise.Data.IRepositories;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FleetLocateController : ControllerBase
{
    private readonly IFleetLocateService _fleetLocateService;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IRoadReadyService _roadReadyService;
    private readonly IUnitOfWork _unitOfWork;

    public FleetLocateController(IFleetLocateService service, IGoogleMapsService googleMapsService,
                                 IRoadReadyService roadReadyService, IUnitOfWork unitOfWork)
    {
        _fleetLocateService = service;
        _googleMapsService = googleMapsService;
        _roadReadyService = roadReadyService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("GetTrailer")]
    public async Task<IActionResult> GetTrailer(int trailerId)
    {
        string[] inclusion = {"Landmark"};
        return Ok(await _unitOfWork.TrailerRepository.SelectAsync(trailerId, inclusion));
    }

    [HttpGet("GetTrailers")]
    public IActionResult GetTrailers()
    {
        var assets = _unitOfWork.TrailerRepository.SelectAll(includes: new string[] { "Landmark" });
        return Ok(assets);
    }

    [HttpGet("GetDistance")]
    public async Task<IActionResult> GetDistance(string origin, string destination)
    {
        var distance = await _googleMapsService.GetDistanceAsync(origin, destination);
        return Ok(distance);
    }

    [HttpGet("Geocode")]
    public async Task<IActionResult> GeoCode(string location, bool reverse)
    {
        return Ok((await _googleMapsService.GetGeocodingAsync(location, reverse)).ToString());
    }

    [HttpGet("StaticMap")]
    public async Task<IActionResult> StaticMap(string location)
    {
        return Ok(await _googleMapsService.GetStaticMapAsync(location));
    }

    [HttpPost("StaticMap2")]
    public async Task<IActionResult> StaticMap2(string center, [FromForm]string[] objects)
    {
        return Ok(await _googleMapsService.GetStaticMapAsync(center, objects));
    }

    [HttpGet("RoadReady")]
    public async Task<ActionResult<IReadOnlyList<TrailerStateDto>>> RoadReady()
    {
        return Ok(await _roadReadyService.GetTrailersStatesAsync());
    }
}
