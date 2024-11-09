using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Brokers.APIs.DitatTms;
using RouteWise.Service.Brokers.APIs.Samsara;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Controllers;

public class TestController : BaseController
{
    private readonly ITrailerService _trailerService;
    private readonly ILandmarkService _landmarkService;
    private readonly ITruckService _truckService;
    private readonly IDitatTmsApiBroker _ditatTmsService;
    private readonly ISamsaraApiBroker _samsaraService;
    private readonly ITelegramBotClient _botClient;

    public TestController(ITrailerService trailerService, ILandmarkService landmarkService,
        ITruckService truckService, IDitatTmsApiBroker ditatTmsService, ISamsaraApiBroker samsaraService, ITelegramBotClient botClient)
    {
        _trailerService = trailerService;
        _landmarkService = landmarkService;
        _truckService = truckService;
        _ditatTmsService = ditatTmsService;
        _samsaraService = samsaraService;
        _botClient = botClient;
    }

    [HttpGet("GetTrips")]
    public async Task<IActionResult> GetTrips()
    {
        var result = await _ditatTmsService.GetTrucksStateWhichHasLoadsAsync();
        return Ok(result);
        //var r = string.Join('\n', result.Split('\n')[..20]);
        //await _botClient.SendTextMessageAsync(6877143602, r, parseMode: ParseMode.Html);
        //return Ok();
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
        return Ok(await _truckService.GetByNameAsync(name));
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

    [HttpGet("trucks")]
    public async Task<IActionResult> GetTrucks()
    {
        return Ok(_samsaraService.GetTruckStateByNameAsync(""));
    }
}
