using RouteWise.Service.Services.FleetLocate;

class Program
{
    static async Task Main(string[] args)
    {
        string login = "SpireonIntegration@mcoexpress.com";
        string password = "Welcome1234";
        string accountId = "1685539";

        FleetLocateService service = new FleetLocateService(login: login, password: password, accountId: accountId);

        var result = await service.GetAssetsAsync();
        Console.WriteLine(result);
    }
}