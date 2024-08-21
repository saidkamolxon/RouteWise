namespace RouteWise.Service.Interfaces;

public interface IDitatTmsService
{
    Task<string> GetAvailableTrucksAsync(CancellationToken cancellationToken = default);
}
