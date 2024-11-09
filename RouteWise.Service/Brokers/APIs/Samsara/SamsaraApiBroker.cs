using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Brokers.APIs.Samsara;

public class SamsaraApiBroker(IConfiguredClients clients, IConfiguredMappers mappers) : ISamsaraApiBroker
{
    private readonly IRestClient client = clients.SamsaraClient;
    private readonly IMapper mapper = mappers.SamsaraMapper;

    public async Task<ICollection<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/stats")
            .AddParameter("types", "gps");

        var trucks = await GetDataAsync<JArray>(request, cancellationToken);

        return [];
    }

    public async Task<string> GetDriverByTruckNameAsync(string truck, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles");

        var data = await GetDataAsync<JArray>(request, cancellationToken);

        var driver = data.FirstOrDefault(d => d.Value<string>("name") == truck);

        return driver.Value<JToken>("staticAssignedDriver").Value<string>("name");
    }

    public async Task<string> GetDriverByVehicleIdAsync(string vehicleId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/{id}")
            .AddUrlSegment("id", vehicleId);

        var data = await GetDataAsync<JToken>(request, cancellationToken);
        return data.Value<JToken>("staticAssignedDriver").Value<string>("name");
    }

    public async Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return (await GetAllTrucksStatesAsync(cancellationToken))
            .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<TruckResultDto> GetVehicleByIdAsync(string vehicleId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("fleet/vehicles/{id}")
            .AddUrlSegment("id", vehicleId);

        var data = await GetDataAsync<JToken>(request, cancellationToken);

        throw new NotImplementedException();
    }

    private async Task<T> GetDataAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : JToken
    {
        var response = await client.GetAsync(request, cancellationToken);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var content = JObject.Parse(response.Content);
            return content.Value<T>("data");
        }
        throw new Exception("An error occured while fetching data from Samsara-Cloud-Api");
    }
}
