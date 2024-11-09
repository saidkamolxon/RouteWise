using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;

namespace RouteWise.Data.IRepositories;

public interface ILandmarkRepository : IRepository<Landmark>
{
    IQueryable<Landmark> GetClosestLandmarks(Coordination coordinates, double rangeInDegrees = default);
}
