using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;

namespace RouteWise.Bot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FleetLocateController : ControllerBase
    {
        private IFleetLocateService _service;

        public FleetLocateController(IFleetLocateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrailers()
        {
            var assets = await _service.GetAssetsAsync();
            return Ok(assets);
        }    
    }
}
