using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.Samsara;

public class SamsaraService : ISamsaraService
{
    private readonly IRestClient _client;

    public SamsaraService(SamsaraApiCredentials credentials)
    {
        _client = new RestClient(credentials.BaseUrl);
        _client.AddDefaultHeader("Authorization", $"Bearer {credentials.Token}");
        _client.AddDefaultHeader("Accept", "application/json");
    }

    public async Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/stats")
            .AddParameter("types", "gps");

        var trucks = await this.GetDataAsync(request, cancellationToken);

        return [];
    }

    public async Task<string> GetDriverByTruckNameAsync(string truck, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles");

        var data = await this.GetDataAsync(request, cancellationToken);

        var driver = data.FirstOrDefault(d => d.Value<string>("name") == truck);

        return driver.Value<JToken>("staticAssignedDriver").Value<string>("name");
    }

    public async Task<string> GetDriverByVehicleIdAsync(string vehicleId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/{id}")
            .AddUrlSegment("id", vehicleId);

        var data = await this.GetDataAsync(request, cancellationToken);
        return data.Value<JToken>("staticAssignedDriver").Value<string>("name");
    }

    public async Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return (await GetAllTrucksStatesAsync())
            .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<JArray> GetDataAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(request, cancellationToken);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var content = JObject.Parse(response.Content);
            return content.Value<JArray>("data");
        }
        throw new Exception("A bad request...");
    }
}
