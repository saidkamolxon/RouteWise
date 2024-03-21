using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

[Route("api/[action]")]
[ApiController]
public class FleetLocateController : ControllerBase
{
    private readonly IFleetLocateService _service;
    private readonly IGoogleMapsService _googleMapsService;

    public FleetLocateController(IFleetLocateService service, IGoogleMapsService googleMapsService)
    {
        _service = service;
        _googleMapsService = googleMapsService;
    }

    [HttpGet("GetTrailers")]
    public async Task<IActionResult> GetTrailers()
    {
        var assets = await _service.GetAssetsAsync();
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
}
