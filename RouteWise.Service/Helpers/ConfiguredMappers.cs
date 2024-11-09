using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Helpers;

public class ConfiguredMappers : IConfiguredMappers
{
    public IMapper RoadReadyMapper => roadReadyMapper.Value;
    public IMapper FleetLocateTrailerStateMapper => fleetLocateTrailerStateMapper.Value;
    public IMapper FleetLocateLandmarkUpdateMapper => fleetLocateLandmarkUpdateMapper.Value;
    public IMapper SamsaraMapper => samsaraMapper.Value;
    public IMapper SwiftEldMapper => swiftEldMapper.Value;

    private static Lazy<IMapper> roadReadyMapper => new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<JToken, TrailerStateDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["trailerName"].ToString()))
              .ForMember(dest => dest.Address, opt => opt.MapFrom<TrailerAddressResolver>())
              .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TrailerCoordinatesResolver>())
              .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TrailerLastEventAtResolver>())
              .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src["landmarkTrailerState"].ToString().Equals("InMotion")));
        });

        return config.CreateMapper();
    });

    private static Lazy<IMapper> fleetLocateTrailerStateMapper => new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<JToken, TrailerStateDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value<string>("name")))
              .ForMember(dest => dest.Address, opt => opt.MapFrom<TrailerAddressResolver>())
              .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TrailerCoordinatesResolver>())
              .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TrailerLastEventAtResolver>())
              .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src.Value<bool>("moving")));
        });

        return config.CreateMapper();
    });

    private static Lazy<IMapper> fleetLocateLandmarkUpdateMapper => new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<JToken, LandmarkUpdateDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value<string>("name")))
                .ForMember(dest => dest.Address, opt => opt.MapFrom<LandmarkAddressResolver>())
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<LandmarkCoordinatesResolver>())
                .ForMember(dest => dest.BorderPoints, opt => opt.MapFrom<LandmarkBorderPointsResolver>());
        });

        return config.CreateMapper();
    });

    private static Lazy<IMapper> samsaraMapper => new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<JToken, TruckStateDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["name"].ToString()))
              .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src["gps"]["reverseGeo"]["formattedLocation"].ToString()))
              .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TruckCoordinatesResolver>())
              .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TruckLastEventAtResolver>())
              .ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src["gps"]["speedMilesPerHour"].ToString() + " mph"));
        });

        return config.CreateMapper();
    });

    private static Lazy<IMapper> swiftEldMapper => new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<JToken, TruckStateDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value<string>("truckNumber")))
                .ForMember(dest => dest.License, opt => opt.MapFrom(src => src.Value<string>("licensePlate")))
                .ForMember(dest => dest.Vin, opt => opt.MapFrom(src => src.Value<string>("vin")))
                .ForMember(dest => dest.Odometer, opt => opt.MapFrom(src => src.Value<string>("odometer")))
                .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.Value<int?>("driverId")))
                .ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.Value<string>("speed")))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TruckCoordinatesResolver>())
                .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TruckLastEventAtResolver>());
        });

        return config.CreateMapper();
    });
}
