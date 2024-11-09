using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Brokers.APIs.SwiftEld;

public class SwiftEldApiBroker(IConfiguredClients clients, IConfiguredMappers mappers) : ISwiftEldApiBroker
{
    private readonly IRestClient client = clients.SwiftEldClient;
    private readonly IMapper mapper = mappers.SwiftEldMapper;

    public async Task<IEnumerable<string>> GetAllTruckNumbersAsync(CancellationToken cancellationToken = default)
    {
        var content = await getDataAsync("asset-position/truck-list", cancellationToken);
        return content.Select(x => x.Value<string>("truckNumber"));
    }

    public async Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default)
    {
        var content = await getDataAsync("asset-position/truck-list", cancellationToken);
        return Map(content);
    }

    public async Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var truckStates = await GetAllTrucksStatesAsync(cancellationToken);
        return truckStates.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<JArray> getDataAsync(string source, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync(new RestRequest(source), cancellationToken);
        
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JArray.Parse(response.Content);
        
        throw new Exception("An error occured while fetching data from Swift-ELD-Api");
    }

    private IEnumerable<TruckStateDto> Map(JArray trucks)
        => mapper.Map<List<TruckStateDto>>(trucks.Where(t => t.Value<string>("signalTime") != null));
}
