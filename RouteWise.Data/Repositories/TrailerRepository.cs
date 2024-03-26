using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Repositories;

public class TrailerRepository : Repository<Trailer>, ITrailerRepository
{
    public TrailerRepository(AppDbContext appDbContext) : base(appDbContext) { }
}
