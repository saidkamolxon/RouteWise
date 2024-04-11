using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext) : base(appDbContext) { }
}
