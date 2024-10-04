using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Repositories;

public class TruckRepository(AppDbContext appDbContext) : Repository<Truck>(appDbContext), ITruckRepository
{
}

