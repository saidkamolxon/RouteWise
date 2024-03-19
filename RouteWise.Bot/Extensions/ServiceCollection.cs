using RouteWise.Service.Interfaces;
using RouteWise.Service.Services.FleetLocate;

namespace RouteWise.Bot.Extensions;

public static class ServiceCollection
{
    public static void AddFleetLocate(this IServiceCollection services, IConfiguration configuration)
    {
        var credentials = configuration.GetSection("AccessToExternalAPIs:FleetLocate")
                            .Get<FleetLocateAPICredentials>();
        services.AddScoped<IFleetLocateService, FleetLocateService>(provider =>
        {
            return new FleetLocateService(credentials);
        });
    }
}