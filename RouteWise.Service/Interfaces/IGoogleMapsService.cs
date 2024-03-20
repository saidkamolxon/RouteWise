namespace RouteWise.Service.Interfaces;

public interface IGoogleMapsService
{
    Task<object> GetStaticMapAsync(string coordinates);
    Task<object> GetStaticMapAsync(string center, string[] objects);
    Task<object> GetGeocodingAsync(string location, bool reverse);
    Task<string> GetDistanceAsync(string origin, string destination);
}