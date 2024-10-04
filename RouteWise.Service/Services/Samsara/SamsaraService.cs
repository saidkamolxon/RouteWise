using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.Samsara;

public class SamsaraService : ISamsaraService
{
    private readonly IRestClient _client;
    private readonly IMapper _mapper;

    public SamsaraService(IConfiguredClients configuredClients)
    {
        _client = configuredClients.SamsaraClient;
        _mapper = ConfigureMapper();
    }

    private IMapper ConfigureMapper()
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
    }



    public async Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/stats")
            .AddParameter("types", "gps");

        var trucks = await this.GetDataAsync<JArray>(request, cancellationToken);

        return [];
    }

    public async Task<string> GetDriverByTruckNameAsync(string truck, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles");

        var data = await this.GetDataAsync<JArray>(request, cancellationToken);

        var driver = data.FirstOrDefault(d => d.Value<string>("name") == truck);

        return driver.Value<JToken>("staticAssignedDriver").Value<string>("name");
    }

    public async Task<string> GetDriverByVehicleIdAsync(string vehicleId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/{id}")
            .AddUrlSegment("id", vehicleId);

        var data = await this.GetDataAsync<JToken>(request, cancellationToken);
        return data.Value<JToken>("staticAssignedDriver").Value<string>("name");
    }

    public async Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return (await GetAllTrucksStatesAsync(cancellationToken))
            .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<TruckResultDto> GetVehicleById(string vehicleId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/{id}")
            .AddUrlSegment("id", vehicleId);

        var data = await this.GetDataAsync<JToken>(request, cancellationToken);

        throw new NotImplementedException();
    }

    private async Task<T> GetDataAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : JToken
    {
        var response = await _client.GetAsync(request, cancellationToken);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var content = JObject.Parse(response.Content);
            return content.Value<T>("data");
        }
        throw new Exception("A bad request...");
    }
}
