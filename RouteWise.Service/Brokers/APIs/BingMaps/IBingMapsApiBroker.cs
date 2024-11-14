namespace RouteWise.Service.Brokers.APIs.BingMaps;

public interface IBingMapsApiBroker
{
    Task<string> GetStaticMapAsync(string coordinates, CancellationToken cancellationToken = default);
}