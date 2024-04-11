using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;

namespace RouteWise.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context, ITrailerRepository trailerRepository,
        ILandmarkRepository landmarkRepository, IUserRepository userRepository)
    {
        _context = context;
        TrailerRepository = trailerRepository;
        LandmarkRepository = landmarkRepository;
        UserRepository = userRepository;
    }

    public ILandmarkRepository LandmarkRepository { get; set; }
    public ITrailerRepository TrailerRepository { get; }
    public IUserRepository UserRepository { get; }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
    public void Dispose() => GC.SuppressFinalize(true);
}
