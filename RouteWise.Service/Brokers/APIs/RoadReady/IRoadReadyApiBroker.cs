using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Brokers.APIs.RoadReady;

public interface IRoadReadyApiBroker
{
    Task<ICollection<TrailerStateDto>> GetTrailersStatesAsync(CancellationToken cancellationToken = default);
}
