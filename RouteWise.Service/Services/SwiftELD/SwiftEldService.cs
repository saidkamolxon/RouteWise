using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.SwiftELD;

public class SwiftEldService : ISwiftEldService
{
    private readonly IRestClient _client;
    private readonly IMapper _mapper;

    public SwiftEldService(SwiftEldApiCredentials credentials)
    {
        _client = new RestClient(credentials.BaseUrl);
        _client.AddDefaultParameter("token", credentials.Token);
        _mapper = CreateAndConfigureMapper();
    }

    private static IMapper CreateAndConfigureMapper()
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
    }

    public async Task<IEnumerable<string>> GetAllTruckNumbersAsync(CancellationToken cancellationToken = default)
    {
        var content = await GetDataAsync("asset-position/truck-list");
        return content.Select(x => x.Value<string>("truckNumber"));
    }

    public async Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default)
    {
        var content = await GetDataAsync("asset-position/truck-list");
        return Map(content);
    }

    public async Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return (await GetAllTrucksStatesAsync())
            .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<JArray> GetDataAsync(string source, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(new RestRequest(source), cancellationToken);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JArray.Parse(response.Content);
        throw new Exception("A bad request...");
    }

    private IEnumerable<TruckStateDto> Map(JArray trucks)
        => _mapper.Map<List<TruckStateDto>>(trucks.Where(t => t.Value<string>("signalTime") != null));
}
