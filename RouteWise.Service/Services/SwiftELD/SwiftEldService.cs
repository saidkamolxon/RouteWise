using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.SwiftELD;

public class SwiftEldService : ISwiftEldService
{
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IRestClient _client;
    private readonly IMapper _mapper;

    public SwiftEldService(IGoogleMapsService googleMapsService, SwiftEldApiCredentials credentials)
    {
        _googleMapsService = googleMapsService;
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

    public async Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync()
    {
        var content = await GetDataAsync("asset-position/truck-list");
        var result = await MapAsync(content);
        return result;
    }

    public Task<TruckStateDto> GetTruckStateByNameAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<JArray> GetDataAsync(string source)
    {
        var response = await _client.GetAsync(new RestRequest(source));
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JArray.Parse(response.Content);
        throw new Exception("A bad request...");
    }

    private async Task<IEnumerable<TruckStateDto>> MapAsync(JArray trucks)
    {
        var result = new List<TruckStateDto>();
        
        foreach (var truck in trucks.Where(t => t.Value<string>("signalTime") is not null))
        {
            var dto = _mapper.Map<TruckStateDto>(truck);
            dto.Address = await _googleMapsService.GetReverseGeocodingAsync(dto.Coordinates.ToString());
            result.Add(dto);
        }
        return result;
    }
}
