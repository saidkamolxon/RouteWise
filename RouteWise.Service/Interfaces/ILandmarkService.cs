using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Landmark;

namespace RouteWise.Service.Interfaces;

public interface ILandmarkService
{
    Task<IEnumerable<LandmarkResultDto>> GetLandmarksByNameAsync(string name);
    Task<IEnumerable<LandmarkResultDto>> GetAllLandmarksAsync();
    Task UpdateLandmarksAsync();
    Task<int?> GetLandmarkIdOrDefaultAsync(string state, Coordinate coordinates);
}
