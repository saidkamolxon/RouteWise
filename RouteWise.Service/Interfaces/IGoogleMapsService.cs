using RouteWise.Domain.Models;

namespace RouteWise.Service.Interfaces;

public interface IGoogleMapsService
{
    Task<string> GetStaticMapAsync(string coordinates);
    Task<object> GetStaticMapAsync(string center, string[] objects);
    Task<object> GetGeocodingAsync(string location, bool reverse);
    Task<string> GetReverseGeocodingAsync(string coordinates);
    Task<string> GetDistanceAsync(string origin, string destination);
}