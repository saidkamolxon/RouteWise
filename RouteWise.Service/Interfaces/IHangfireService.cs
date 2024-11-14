namespace RouteWise.Service.Interfaces;

public interface IHangfireService
{
    void Start(CancellationToken cancellationToken = default);
}