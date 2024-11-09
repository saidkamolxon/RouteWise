using RouteWise.Domain.Models;

namespace RouteWise.Service.Brokers.APIs.GoogleMaps;

public interface IGoogleMapsApiBroker
{
    Task<string> GetStaticMapAsync(string coordinates, string iconUrl = null, CancellationToken cancellationToken = default);
    Task<string> GetStaticMapAsync(string center, string[] objects, CancellationToken cancellationToken = default);
    Task<object> GetGeocodingAsync(string location, bool reverse, CancellationToken cancellationToken = default);
    Task<string> GetReverseGeocodingAsync(string coordinates, CancellationToken cancellationToken = default);
    Task<string> GetDistanceAsync(string origin, string destination, CancellationToken cancellationToken = default);
}