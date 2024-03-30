using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Interfaces;

public interface IFleetLocateService
{
    Task<object> GetAssetsAsync();
    Task<IEnumerable<LandmarkUpdateDto>> GetLandmarksAsync();
    Task<object> GetAssetsStatusesAsync();
    Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync();
}
