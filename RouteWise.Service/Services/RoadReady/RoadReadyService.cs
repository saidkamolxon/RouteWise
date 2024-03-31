using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.RoadReady;

public class RoadReadyService : IRoadReadyService
{
    private readonly IRestClient _client;
    private readonly IMapper _mapper;

    public RoadReadyService(RoadReadyApiCredentials credentials)
    {
        _client = new RestClient(credentials.BaseUrl);
        _client.AddDefaultHeader("x-api-key", credentials.Token);
        _mapper = CreateAndConfigureMapper();
    }

    private static IMapper CreateAndConfigureMapper()
    {
        var config = new MapperConfiguration(cfg => {
          cfg.CreateMap<JToken, TrailerStateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["trailerName"].ToString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom<TrailerAddressResolver>())
            .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TrailerCoordinatesResolver>())
            .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TrailerDateTimeResolver>())
            .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src["landmarkTrailerState"].ToString().Equals("InMotion")));});
        return config.CreateMapper();
    }

    public async Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync()
    {
        var content = await GetDataAsync("fleet_trailer_states");
        var mapped = Map(content.Value<JArray>("data"));
        return mapped;
    }

    private async Task<JObject> GetDataAsync(string source)
    {
        var response = await _client.GetAsync(new RestRequest(source));
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JObject.Parse(response.Content);
        throw new Exception("A bad request...");
    }

    private IEnumerable<TrailerStateDto> Map(JArray trailers)
    {
        var result = new List<TrailerStateDto>();
        foreach (var trailer in trailers)
        {
            var attr = trailer["attributes"];
            var dto = _mapper.Map<TrailerStateDto>(attr);
            result.Add(dto);
        }
        return result;
    }
}
