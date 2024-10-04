using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;

namespace RouteWise.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context, ITrailerRepository trailerRepository,
        ILandmarkRepository landmarkRepository, IUserRepository userRepository, IStateRepository stateRepository, ITruckRepository truckRepository)
    {
        _context = context;
        TrailerRepository = trailerRepository;
        LandmarkRepository = landmarkRepository;
        UserRepository = userRepository;
        StateRepository = stateRepository;
        TruckRepository = truckRepository;
    }

    public ILandmarkRepository LandmarkRepository { get; set; }
    public IStateRepository StateRepository { get; }
    public ITrailerRepository TrailerRepository { get; }
    public ITruckRepository TruckRepository { get; }
    public IUserRepository UserRepository { get; }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
    
    public void Dispose() => GC.SuppressFinalize(this);
}
