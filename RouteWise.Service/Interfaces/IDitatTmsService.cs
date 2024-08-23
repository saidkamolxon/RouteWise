namespace RouteWise.Service.Interfaces;

public interface IDitatTmsService
{
    Task<string> GetAvailableTrucksAsync(bool withDrivers = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Uri>> GetTrailerDocsAsync(string trailer, CancellationToken cancellationToken = default);
}
