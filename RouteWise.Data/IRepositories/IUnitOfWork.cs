namespace RouteWise.Data.IRepositories;

public interface IUnitOfWork : IDisposable
{
    ITrailerRepository TrailerRepository { get; }
    Task SaveAsync();
}
