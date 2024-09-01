using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.FleetLocate;

public class FleetLocateService : IFleetLocateService
{
    private readonly HttpClient _client;
    private readonly int _tries;
    private readonly IMapper _trailerStateMapper;
    private readonly IMapper _landmarkUpdateMapper;

    public FleetLocateService(IHttpClientFactory httpClientFactory)
    {
        _tries = 10;
        _client = httpClientFactory.CreateClient("FleetLocateApiClient");
        _trailerStateMapper = ConfigureTrailerStateMapper();
        _landmarkUpdateMapper = ConfigureLandmarkUpdateMapper();
    }

    private static IMapper ConfigureTrailerStateMapper()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<JToken, TrailerStateDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value<string>("name")))
              .ForMember(dest => dest.Address, opt => opt.MapFrom<TrailerAddressResolver>())
              .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TrailerCoordinatesResolver>())
              .ForMember(dest => dest.LastEventAt, opt => opt.MapFrom<TrailerLastEventAtResolver>())
              .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src.Value<bool>("moving")));
        });
        return config.CreateMapper();
    }

    private static IMapper ConfigureLandmarkUpdateMapper()
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
    }

    public async Task<object> GetAssetsAsync()
    {
        var result = await this.GetDataAsync(url: "asset");
        return $"{result}";
    }

    public async Task<dynamic> GetAssetsStatusesAsync()
        => await this.GetDataAsync(url: "assetStatus");

    public async Task<IEnumerable<LandmarkUpdateDto>> GetLandmarksAsync()
    {
        var landmarks = await this.GetDataAsync(url: "landmark");
        return await this.MapToLandmarkUpdateDtoAsync(landmarks);
    }

    public async Task<dynamic> GetLandmarksStatusesAsync()
        => await this.GetDataAsync(url: "landmarkStatus");

    public async Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync()
    {
        var data = await this.GetDataAsync(url: "assetStatus");
        return await MapToTrailerStateDtoAsync(data.Where(x =>
            !string.IsNullOrEmpty(x.Value<string>("name")) &&
            !string.IsNullOrEmpty(x.Value<string>("eventDateTime"))));
    }

    #region Encapsulated methods --->>
    private async Task<IEnumerable<JToken>> GetDataAsync(string url, string param = "data")
    {
        var tries = _tries;
        while (tries > 0)
        {
            JObject jsonResponse = await GetJsonResponseAsync(url);
            if ((bool)jsonResponse["success"])
                return jsonResponse.Value<IEnumerable<JToken>>(param);
            await Task.Delay(1000);
            tries--;
        }
        throw new Exception(); //TODO need to edit this exception in proper way
    }

    private async Task<dynamic> GetJsonResponseAsync(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<dynamic>(responseBody);
    }

    private Task<IEnumerable<TrailerStateDto>> MapToTrailerStateDtoAsync(IEnumerable<JToken> trailers)
    {
        return Task.FromResult<IEnumerable<TrailerStateDto>>(_trailerStateMapper.Map<List<TrailerStateDto>>(trailers));
    }

    private Task<IEnumerable<LandmarkUpdateDto>> MapToLandmarkUpdateDtoAsync(IEnumerable<JToken> landmarks)
    {
        return Task.FromResult<IEnumerable<LandmarkUpdateDto>>(_landmarkUpdateMapper.Map<List<LandmarkUpdateDto>>(landmarks));
    }
    #endregion
}