namespace RouteWise.Data.IRepositories;

public interface IUnitOfWork : IDisposable
{
    ILandmarkRepository LandmarkRepository { get; }
    ITrailerRepository TrailerRepository { get; }
    ITruckRepository TruckRepository { get; }
    IUserRepository UserRepository { get; }
    IStateRepository StateRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
}
