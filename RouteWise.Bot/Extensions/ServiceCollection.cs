using RouteWise.Service.Interfaces;
using RouteWise.Service.Services.FleetLocate;
using RouteWise.Service.Services.GoogleMaps;
using RouteWise.Service.Services.RoadReady;

namespace RouteWise.Bot.Extensions;

public static class ServiceCollection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddFleetLocate(services, configuration);
        AddGoogleMaps(services, configuration);
        AddRoadReady(services, configuration);
        
    }

    private static void AddGoogleMaps(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGoogleMapsService, GoogleMapsService>(provider =>
            new GoogleMapsService(configuration.GetSection("AccessToExternalAPIs:GoogleMaps")
                                               .Get<GoogleMapsApiCredentials>())
        );
    }

    private static void AddFleetLocate(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFleetLocateService, FleetLocateService>(provider =>
            new FleetLocateService(configuration.GetSection("AccessToExternalAPIs:FleetLocate")
                                                .Get<FleetLocateApiCredentials>())
        );
    }

    private static void AddRoadReady(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRoadReadyService, RoadReadyService>(provider =>
            new RoadReadyService(configuration.GetSection("AccessToExternalAPIs:RoadReady")
                                              .Get<RoadReadyApiCredentials>())
        );
    }
}