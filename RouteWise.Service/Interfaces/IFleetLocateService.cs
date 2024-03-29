using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Interfaces;

public interface IFleetLocateService
{
    Task<object> GetAssetsAsync();
    Task<object> GetLandmarksAsync();
    Task<object> GetAssetsStatusesAsync();
    Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync();
}
