using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.RoadReady;

public class RoadReadyService : IRoadReadyService
{
    private readonly IRestClient _client;
    private readonly IRepository<Trailer> _repo;
    private readonly int _tries = 10;
    private readonly IMapper _mapper;

    public RoadReadyService(RoadReadyApiCredentials credentials, IRepository<Trailer> repo, IMapper mapper)
    {
        _client = new RestClient("https://api.roadreadysystem.com/jsonapi");
        _client.AddDefaultHeader("x-api-key", credentials.Token);
        _repo = repo;
        _mapper = mapper;
    }

    public async Task LoadTrailersDataToDatabaseAsync()
    {
        var result = await GetTrailersDataAsync();
        WriteToDatabase(result["data"]);
    }

    private async Task<JObject> GetTrailersDataAsync()
    {
        var request = new RestRequest("fleet_trailer_states");
        var result = await _client.GetAsync(request);
        if(result.IsSuccessful)
            return JObject.Parse(result.Content);
        throw new Exception(""); //TODO must edit this line
    }

    private void WriteToDatabase(JToken data)
    {
        foreach (var tdata in data)
        {
            var attr = tdata["attributes"];
            var dto = new TrailerUpdateDto
            {
                Name = attr["trailerName"].ToString(),
                Address = attr["location"].ToString(),
                Coordinates = $"{attr["latitude"]},{attr["longitude"]}",
                LastEventDate = DateTime.Parse(attr["lastEvent"]["messageDate"].ToString()),
                IsMoving = attr["landmarkTrailerState"].ToString().Equals("InMotion"),
                LandmarkId = 1 //TODO must edit the line
            };

            _repo.Update(_mapper.Map<Trailer>(dto));
        };
        
    }
}
