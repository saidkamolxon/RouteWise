namespace RouteWise.Service.Brokers.APIs.GoogleMaps;

public record GoogleMapsApiCredentials
{
    public string BaseUrl { get; set; }
    public string Token { get; set; }
}
