using RouteWise.Domain.Models;

namespace RouteWise.Service.Interfaces;

public interface ILandmarkService
{
    Task<int> GetLandmarkIdOrDefaultAsync(string state, Coordinate coordinates);
}
