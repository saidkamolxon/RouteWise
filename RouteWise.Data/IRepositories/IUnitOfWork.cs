namespace RouteWise.Data.IRepositories;

public interface IUnitOfWork : IDisposable
{
    ILandmarkRepository LandmarkRepository { get; }
    ITrailerRepository TrailerRepository { get; }
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}
