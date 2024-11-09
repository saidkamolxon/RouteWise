using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Brokers.APIs.FleetLocate;

public class FleetLocateApiBroker(IConfiguredClients clients, IConfiguredMappers mappers) : IFleetLocateApiBroker
{
    private readonly IRestClient client = clients.FleetLocateClient;
    private readonly IMapper trailerStateMapper = mappers.FleetLocateTrailerStateMapper;
    private readonly IMapper landmarkUpdateMapper = mappers.FleetLocateLandmarkUpdateMapper;

    public async Task<object> GetAssetsAsync(CancellationToken cancellationToken = default)
    {
        var result = await getDataAsync(url: "asset");
        return $"{result}";
    }

    public async Task<dynamic> GetAssetsStatusesAsync(CancellationToken cancellationToken = default)
        => await getDataAsync(url: "assetStatus");

    public async Task<ICollection<LandmarkUpdateDto>> GetLandmarksAsync(CancellationToken cancellationToken = default)
    {
        var landmarks = await getDataAsync(url: "landmark");
        return await mapToLandmarkUpdateDtoAsync(landmarks);
    }

    public async Task<dynamic> GetLandmarksStatusesAsync()
        => await getDataAsync(url: "landmarkStatus");

    public async Task<ICollection<TrailerStateDto>> GetTrailersStatesAsync(CancellationToken cancellationToken = default)
    {
        var data = await getDataAsync(url: "assetStatus");
        return await mapToTrailerStateDtoAsync(data.Where(x =>
            !string.IsNullOrEmpty(x.Value<string>("name")) &&
            !string.IsNullOrEmpty(x.Value<string>("eventDateTime"))));
    }

    #region Encapsulated methods --->>
    private async Task<IEnumerable<JToken>> getDataAsync(string url, string param = "data")
    {
        var tries = 10;
        while (tries > 0)
        {
            JObject jsonResponse = await getJsonResponseAsync(url);
            if ((bool)jsonResponse["success"])
                return jsonResponse.Value<IEnumerable<JToken>>(param);
            await Task.Delay(1000);
            tries--;
        }
        throw new Exception("An error occured while fetching data from Spireon's Fleet-Locate-Api");
    }

    private async Task<dynamic> getJsonResponseAsync(string url)
    {
        var response = await client.GetAsync(new RestRequest(url));

        if (response.IsSuccessful)
            return JsonConvert.DeserializeObject<dynamic>(response.Content);

        throw new Exception("An error occured while fetching data from Spireon's Fleet-Locate-Api");
    }

    private Task<ICollection<TrailerStateDto>> mapToTrailerStateDtoAsync(IEnumerable<JToken> trailers)
        => Task.FromResult<ICollection<TrailerStateDto>>(trailerStateMapper.Map<List<TrailerStateDto>>(trailers));

    private Task<ICollection<LandmarkUpdateDto>> mapToLandmarkUpdateDtoAsync(IEnumerable<JToken> landmarks)
        => Task.FromResult<ICollection<LandmarkUpdateDto>>(landmarkUpdateMapper.Map<List<LandmarkUpdateDto>>(landmarks));

    #endregion
}