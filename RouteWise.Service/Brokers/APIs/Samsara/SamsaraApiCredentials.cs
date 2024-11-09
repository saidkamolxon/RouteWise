namespace RouteWise.Service.Brokers.APIs.Samsara;

public record SamsaraApiCredentials
{
    public string BaseUrl { get; set; }
    public string Token { get; set; }
}
