namespace RouteWise.Service.Brokers.APIs.RoadReady;

public record RoadReadyApiCredentials
{
    public string BaseUrl { get; set; }
    public string Token { get; set; }
}
