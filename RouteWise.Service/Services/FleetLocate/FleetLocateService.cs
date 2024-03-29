using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace RouteWise.Service.Services.FleetLocate;

public class FleetLocateService : IFleetLocateService
{
    private readonly HttpClient _client;
    private readonly int _tries;
    private readonly IMapper _mapper;

    public FleetLocateService(FleetLocateApiCredentials credentials)
    {
        _tries = 10;
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.us.spireon.com/api/");
        Authorize(credentials);
        _mapper = CreateAndConfigureMapper();
    }

    private static IMapper CreateAndConfigureMapper()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<JToken, TrailerStateDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value<string>("name")))
              .ForMember(dest => dest.Address, opt => opt.MapFrom<TrailerAddressResolver>())
              .ForMember(dest => dest.Coordinates, opt => opt.MapFrom<TrailerCoordinatesResolver>())
              .ForMember(dest => dest.LastEventDate, opt => opt.MapFrom(src => 
                  DateTime.ParseExact(src.Value<string>("eventDateTime"),"yyyy-MM-dd HH:mm:ss", null))
               )
              .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src.Value<bool>("moving")));
        });
        return config.CreateMapper();
    }

    private void Authorize(FleetLocateApiCredentials credentials)
    {
        string authString = this.GetAuthString(credentials.Login, credentials.Password);
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {authString}");
        _client.DefaultRequestHeaders.Add("Account", credentials.AccountId);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private string GetAuthString(string login, string password)
    {
        byte[] userBytes = Encoding.ASCII.GetBytes($"{login}:{password}");
        return Convert.ToBase64String(userBytes);
    }

    public async Task<object> GetAssetsAsync()
    {
        var result = await this.GetDataAsync(url: "asset");
        return $"{result}";
    }

    public async Task<dynamic> GetAssetsStatusesAsync()
        => await this.GetDataAsync(url: "assetStatus");

    public async Task<object> GetLandmarksAsync()
        => await this.GetDataAsync(url: "landmark");

    public async Task<dynamic> GetLandmarksStatusesAsync()
        => await this.GetDataAsync(url: "landmarkStatus");

    public async Task<IEnumerable<TrailerStateDto>> GetTrailersStatesAsync()
    {
        var data = await this.GetDataAsync(url: "assetStatus");
        return await MapAsync(data.Where(x =>
            !string.IsNullOrEmpty(x.Value<string>("name")) ||
            !string.IsNullOrEmpty(x.Value<string>("eventDateTime"))));
    }

    #region Encapsulated methods --->>
    private async Task<IEnumerable<JToken>> GetDataAsync(string url, string param = "data")
    {
        int tries = _tries;
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

    private async Task<IEnumerable<TrailerStateDto>> MapAsync(IEnumerable<JToken> trailers)
    {
        return _mapper.Map<List<TrailerStateDto>>(trailers);
    }
    #endregion
}