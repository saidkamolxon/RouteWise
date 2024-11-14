using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Services;
using RouteWise.Data.IRepositories;
using RouteWise.Data.Repositories;
using RouteWise.Service.Brokers.APIs.DitatTms;
using RouteWise.Service.Brokers.APIs.FleetLocate;
using RouteWise.Service.Brokers.APIs.GoogleMaps;
using RouteWise.Service.Brokers.APIs.RoadReady;
using RouteWise.Service.Brokers.APIs.Samsara;
using RouteWise.Service.Brokers.APIs.SwiftEld;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;
using RouteWise.Service.Services;

namespace RouteWise.Bot.Extensions;

public static class ServiceCollection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ILandmarkRepository, LandmarkRepository>();
        services.AddScoped<IStateRepository, StateRepository>();
        services.AddScoped<ITrailerRepository, TrailerRepository>();
        services.AddScoped<ITruckRepository, TruckRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IStateMachine>(provider => new StateMachine(provider));
        services.AddTransient<IConfiguredClients, ConfiguredClients>();
        services.AddSingleton<IConfiguredMappers, ConfiguredMappers>();

        services.AddScoped<ILandmarkService, LandmarkService>();
        services.AddScoped<ITrailerService, TrailerService>();
        services.AddScoped<ITruckService, TruckService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IHangfireService, HangfireService>();
        
        // Api services --->>>
        services.AddScoped<IFleetLocateApiBroker, FleetLocateApiBroker>();
        services.AddScoped<IGoogleMapsApiBroker, GoogleMapsApiBroker>();
        services.AddScoped<IRoadReadyApiBroker, RoadReadyApiBroker>();
        services.AddScoped<ISamsaraApiBroker, SamsaraApiBroker>();
        services.AddScoped<ISwiftEldApiBroker, SwiftEldApiBroker>();
        services.AddScoped<IDitatTmsApiBroker, DitatTmsApiBroker>();
    }
}