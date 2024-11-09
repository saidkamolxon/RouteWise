using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Landmark;

namespace RouteWise.Service.Interfaces;

public interface ILandmarkService
{
    Task<IEnumerable<LandmarkResultDto>> GetLandmarksByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<LandmarkResultDto>> GetAllLandmarksAsync(CancellationToken cancellationToken = default);
    Task UpdateLandmarksAsync(CancellationToken cancellationToken = default);
    Task<int?> GetLandmarkIdOrDefaultAsync(string state, Coordination coordinates, CancellationToken cancellationToken = default);
    Task RemoveRedundantLandmarks(CancellationToken cancellationToken = default);
}
