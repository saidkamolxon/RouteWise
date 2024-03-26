﻿using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Data.IRepositories;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.RoadReady;

public class RoadReadyService : IRoadReadyService
{
    private readonly IRestClient _client;
    private readonly int _tries = 10;
    private readonly IMapper _mapper;
    public readonly ILandmarkService _landmarkService;

    public RoadReadyService(RoadReadyApiCredentials credentials, ILandmarkService landmarkService)
    {
        _client = new RestClient("https://api.roadreadysystem.com/jsonapi");
        _client.AddDefaultHeader("x-api-key", credentials.Token);
        _mapper = CreateAndConfigureMapper();
        _landmarkService = landmarkService;
    }

    private static IMapper CreateAndConfigureMapper()
    {
        var config = new MapperConfiguration(cfg => {
          cfg.CreateMap<JToken, TrailerStateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["trailerName"].ToString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src["location"].ToString()))
            .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => $"{src["latitude"]},{src["longitude"]}"))
            .ForMember(dest => dest.LastEventDate, opt => opt.MapFrom(src => DateTime.ParseExact(src["lastEvent"]["messageDate"].ToString(), "MM-dd-yyyy HH:mm:ss", null)))
            .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src["landmarkTrailerState"].ToString().Equals("InMotion")));});
        return config.CreateMapper();
    }

    public async Task<IReadOnlyList<TrailerStateDto>> GetTrailersStatesAsync()
    {
        var content = await GetDataAsync("fleet_trailer_states");
        var mapped = await MapAsync(content.Value<JArray>("data"));
        return mapped;
    }

    private async Task<JObject> GetDataAsync(string source)
    {
        var response = await _client.GetAsync(new RestRequest(source));
        if (response.IsSuccessful)
            return JObject.Parse(response.Content);
        throw new Exception("A bad request...");
    }

    private async Task<IReadOnlyList<TrailerStateDto>> MapAsync(JArray trailers)
    {
        var result = new List<TrailerStateDto>();
        foreach (var trailer in trailers)
        {
            var attr = trailer["attributes"];
            var dto = _mapper.Map<TrailerStateDto>(attr);
            //dto.LandmarkId = await _landmarkService
            //    .GetLandmarkIdOrDefaultAsync(dto.Address.Substring(dto.Address.Length-13, 2), dto.Coordinates);
            result.Add(dto);
        }
        return result;
    }
}
