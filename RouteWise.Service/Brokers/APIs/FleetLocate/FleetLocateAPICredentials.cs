namespace RouteWise.Service.Brokers.APIs.FleetLocate;

public record FleetLocateApiCredentials
{
    public string BaseUrl { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string AccountId { get; set; }
}