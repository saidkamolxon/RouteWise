using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;

namespace RouteWise.Data.Repositories;

public class UnitOfWork(AppDbContext context,
    ITrailerRepository trailerRepository,
    ILandmarkRepository landmarkRepository,
    IUserRepository userRepository,
    IStateRepository stateRepository,
    ITruckRepository truckRepository) : IUnitOfWork
{
    private readonly AppDbContext context = context;

    public ILandmarkRepository LandmarkRepository { get; set; } = landmarkRepository;
    public IStateRepository StateRepository { get; } = stateRepository;
    public ITrailerRepository TrailerRepository { get; } = trailerRepository;
    public ITruckRepository TruckRepository { get; } = truckRepository;
    public IUserRepository UserRepository { get; } = userRepository;

    public async Task SaveAsync(CancellationToken cancellationToken = default) => await context.SaveChangesAsync(cancellationToken);
    
    public void Dispose() => GC.SuppressFinalize(this);
}
