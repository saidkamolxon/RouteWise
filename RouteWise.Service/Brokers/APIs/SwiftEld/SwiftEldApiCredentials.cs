namespace RouteWise.Service.Brokers.APIs.SwiftEld;

public record SwiftEldApiCredentials
{
    public string BaseUrl { get; set; }
    public string Token { get; set; }
}