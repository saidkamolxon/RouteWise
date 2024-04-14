using Microsoft.Extensions.DependencyInjection;
using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Services;
using RouteWise.Bot.States;
using RouteWise.Data.IRepositories;
using RouteWise.Data.Repositories;
using RouteWise.Service.Interfaces;
using RouteWise.Service.Services;
using RouteWise.Service.Services.FleetLocate;
using RouteWise.Service.Services.GoogleMaps;
using RouteWise.Service.Services.RoadReady;
using RouteWise.Service.Services.SwiftELD;

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

        services.AddScoped<IStateMachine>(p =>
        {
            var unitOfWork = p.GetRequiredService<IUnitOfWork>();
            Func<IState> initialStateFactory = () => new InitialState(p.GetService<IStateMachine>());
            return new StateMachine(initialStateFactory, unitOfWork);
        });
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