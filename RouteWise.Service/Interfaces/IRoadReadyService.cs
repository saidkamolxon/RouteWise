using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Interfaces;

public interface IRoadReadyService
{
    Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync();
}
