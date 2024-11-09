using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Brokers.APIs.RoadReady;

public class RoadReadyApiBroker(IConfiguredClients clients, IConfiguredMappers mappers) : IRoadReadyApiBroker
{
    private readonly IRestClient client = clients.RoadReadyClient;
    private readonly IMapper mapper = mappers.RoadReadyMapper;

    public async Task<ICollection<TrailerStateDto>> GetTrailersStatesAsync(CancellationToken cancellationToken = default)
    {
        var content = await this.getDataAsync("fleet_trailer_states", cancellationToken);
        var mapped = this.mapToTrailerStateDto(content);
        return mapped;
    }

    private async Task<JArray> getDataAsync(string source, CancellationToken cancellationToken = default)
    {
        var response = await this.client.GetAsync(new RestRequest(source), cancellationToken);
       
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JsonConvert.DeserializeObject<JArray>(response.Content);

        throw new Exception("An error occured while fetching data from Road-Ready-Api");
    }

    private ICollection<TrailerStateDto> mapToTrailerStateDto(JArray trailers)
    {
        var list = new List<TrailerStateDto>();
        foreach (var trailer in trailers)
        {
            try
            {
                list.Add(this.mapper.Map<TrailerStateDto>(trailer));
            }
            catch
            {
            }
        }
        return list;
    }
}
