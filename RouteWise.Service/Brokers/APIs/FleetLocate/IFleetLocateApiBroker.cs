using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Brokers.APIs.FleetLocate;

public interface IFleetLocateApiBroker
{
    Task<object> GetAssetsAsync(CancellationToken cancellationToken = default);
    Task<ICollection<LandmarkUpdateDto>> GetLandmarksAsync(CancellationToken cancellationToken = default);
    Task<object> GetAssetsStatusesAsync(CancellationToken cancellationToken = default);
    Task<ICollection<TrailerStateDto>> GetTrailersStatesAsync(CancellationToken cancellationToken = default);
}
