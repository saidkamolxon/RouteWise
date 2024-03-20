using RouteWise.Service.Services.FleetLocate;

class Program
{
    static async Task Main(string[] args)
    {
        var credentials = new FleetLocateApiCredentials
        {
            Login = "spireonintegration@mcoexpress.com",
            Password = "Welcome1234",
            AccountId = "1685539"
        };

        FleetLocateService service = new FleetLocateService(credentials);

        var result = await service.GetLandmarksStatusesAsync();
        Console.WriteLine(result);
    }
}