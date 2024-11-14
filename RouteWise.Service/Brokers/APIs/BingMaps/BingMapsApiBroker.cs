using RestSharp;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Brokers.APIs.BingMaps;

public class BingMapsApiBroker(IConfiguredClients clients) : IBingMapsApiBroker
{
    private readonly IRestClient client = clients.BingMapsClient;
    
    public async Task<string> GetStaticMapAsync(string coordinates, CancellationToken cancellationToken = default)
    {
        var parameters = staticMapsDefaultParameters(coordinates);
        this.client.BuildUri(new RestRequest());
        return "";
    }
    
    private static Dictionary<string, string> staticMapsDefaultParameters(string coordinates)
    {
        return new Dictionary<string, string>
        {
            { "mapSize", "1000x1000" },
            { "pp", $"{coordinates};47;"}
        };
    }
}