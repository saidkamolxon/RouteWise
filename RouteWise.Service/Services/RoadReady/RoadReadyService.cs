using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TrailerLastEventAtResolver>())
            .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src["landmarkTrailerState"].ToString().Equals("InMotion")));});
        return config.CreateMapper();
    }

    public async Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync()
    {
        var content = await GetDataAsync("fleet_trailer_states");
        var mapped = MapToTrailerStateDto(content);
        return mapped;
    }

    private async Task<JArray> GetDataAsync(string source)
    {
        var response = await _client.GetAsync(new RestRequest(source));
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var result = JsonConvert.DeserializeObject<JArray>(response.Content);
            return result;
        }
        
        throw new Exception("A bad request...");
    }

    private IEnumerable<TrailerStateDto> MapToTrailerStateDto(JArray trailers)
    {
        //return _mapper.Map<List<TrailerStateDto>>(trailers);

        var list = new List<TrailerStateDto>();
        foreach (var trailer in trailers)
        {
            try
            {
                list.Add(_mapper.Map<TrailerStateDto>(trailer));
            }
            catch
            {
                //trailer["location"] = 
                //Console.WriteLine();
            }
        }
        return list;
    }
}
