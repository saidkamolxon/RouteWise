namespace RouteWise.Service.Services.DitatTms;

public record DitatTmsApiCredentials
{
    public string BaseUrl { get; set; }
    public string AccountId { get; set; }
    public string ApplicationRole { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}