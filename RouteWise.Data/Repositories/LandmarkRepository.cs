using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Repositories;

public class LandmarkRepository : Repository<Landmark>, ILandmarkRepository
{
    public LandmarkRepository(AppDbContext appDbContext) : base(appDbContext) { }
}
