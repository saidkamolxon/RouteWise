using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers;

public class TrailersController : BaseController
{
    private readonly ITrailerService _service;

    public TrailersController(ITrailerService service)
    {
        _service = service;
    }

    [HttpGet("trailers/{id}")]
    public async Task<IActionResult> GetTrailer(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpGet("trailers")]
    public async Task<IActionResult> GetTrailers()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> AddTrailer(TrailerCreationDto dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }
}
