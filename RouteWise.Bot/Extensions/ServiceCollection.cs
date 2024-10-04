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

        // Api services --->>>
        services.AddScoped<IFleetLocateService, FleetLocateService>();
        services.AddScoped<IGoogleMapsService, GoogleMapsService>();
        services.AddScoped<ILandmarkService, LandmarkService>();
        services.AddScoped<IRoadReadyService, RoadReadyService>();
        services.AddScoped<ISamsaraService, SamsaraService>();
        services.AddScoped<ISwiftEldService, SwiftEldService>();
        services.AddScoped<ITrailerService, TrailerService>();
        services.AddScoped<ITruckService, TruckService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDitatTmsService, DitatTmsService>();
    }
}