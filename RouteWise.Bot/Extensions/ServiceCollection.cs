using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Services;
using RouteWise.Data.IRepositories;
using RouteWise.Data.Repositories;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;
using RouteWise.Service.Services;
using RouteWise.Service.Services.DitatTms;
using RouteWise.Service.Services.FleetLocate;
using RouteWise.Service.Services.GoogleMaps;
using RouteWise.Service.Services.RoadReady;
using RouteWise.Service.Services.Samsara;
using RouteWise.Service.Services.SwiftEld;
using System.Net.Http.Headers;
using System.Text;

namespace RouteWise.Bot.Extensions;

public static class ServiceCollection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ILandmarkService, LandmarkService>();
        services.AddScoped<ITrailerService, TrailerService>();
        services.AddScoped<ITruckService, TruckService>();
        services.AddScoped<ITrailerRepository, TrailerRepository>();
        services.AddScoped<ILandmarkRepository, LandmarkRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStateRepository, StateRepository>();
        services.AddScoped<IUserService, UserService>();
        AddFleetLocate(services, configuration);
        AddGoogleMaps(services, configuration);
        AddRoadReady(services, configuration);
        AddSwiftEld(services, configuration);
        AddDitatTms(services, configuration);
        AddSamsara(services, configuration);
        services.AddScoped<IStateMachine>(provider => new StateMachine(provider));
    }

    private static void AddSwiftEld(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISwiftEldService, SwiftEldService>(provider =>
            new SwiftEldService(configuration.GetSection("AccessToExternalAPIs:SwiftELD")
                                             .Get<SwiftEldApiCredentials>())
        );
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
        var credentials = configuration.GetSection("AccessToExternalAPIs:FleetLocate").Get<FleetLocateApiCredentials>();
        services.AddHttpClient("FleetLocateApiClient", client =>
        {
            client.BaseAddress = new Uri(credentials.BaseUrl);
            string authString = AuthorizationHelper.GetAuthString(credentials.Login, credentials.Password);
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authString}");
            client.DefaultRequestHeaders.Add("Account", credentials.AccountId);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddScoped<IFleetLocateService, FleetLocateService>();
    }

    private static void AddRoadReady(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRoadReadyService, RoadReadyService>(provider =>
            new RoadReadyService(configuration.GetSection("AccessToExternalAPIs:RoadReady")
                                              .Get<RoadReadyApiCredentials>())
        );
    }

    private static void AddDitatTms(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDitatTmsService, DitatTmsService>(provider =>
            new DitatTmsService(configuration.GetSection("AccessToExternalAPIs:DitatTMS")
                                             .Get<DitatTmsApiCredentials>(),
            provider.GetRequiredService<IMemoryCache>()) 
        );
    }

    private static void AddSamsara(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISamsaraService, SamsaraService>(provider =>
            new SamsaraService(configuration.GetSection("AccessToExternalAPIs:Samsara")
                                            .Get<SamsaraApiCredentials>())
            );
    }
    
    
}