using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
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
            cfg.CreateMap<JToken, TruckStateDto>();
                //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value<string>("name")))
                //.ForMember(dest => dest.Address, opt => opt.MapFrom<LandmarkAddressResolver>())
                //.ForMember(dest => dest.Coordinates, opt => opt.MapFrom<LandmarkCoordinatesResolver>())
                //.ForMember(dest => dest.BorderPoints, opt => opt.MapFrom<LandmarkBorderPointsResolver>());
        });
        return config.CreateMapper();
    }

    public async Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync()
    {
        var content = await GetDataAsync("asset-position/truck-list");
        throw new NotImplementedException();
    }

    public Task<TruckStateDto> GetTruckStateByNameAsync()
    {
        throw new NotImplementedException();
    }
    private async Task<JObject> GetDataAsync(string source)
    {
        var response = await _client.GetAsync(new RestRequest(source));
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JObject.Parse(response.Content);
        throw new Exception("A bad request...");
    }
    private IEnumerable<TruckStateDto> Map(JArray trucks)
    {
        var result = new List<TruckStateDto>();
        foreach (var truck in trucks)
        {
            var dto = _mapper.Map<TruckStateDto>(truck);
            result.Add(dto);
        }
        return result;
    }
}
