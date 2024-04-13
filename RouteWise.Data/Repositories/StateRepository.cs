using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Repositories;

public class StateRepository : Repository<State>, IStateRepository
{
    public StateRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}