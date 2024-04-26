﻿using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services.GoogleMaps;

public class GoogleMapsService : IGoogleMapsService
{
    private readonly IRestClient _client;

    public GoogleMapsService(GoogleMapsApiCredentials credentials)
    {
        _client = new RestClient(credentials.BaseUrl);
        _client.AddDefaultParameter("key", credentials.Token);
    }

    public async Task<string> GetDistanceAsync(string origin, string destination)
    {
        var parameters = new Dictionary<string, string>
           {{ "origins", origin },
            { "destinations", destination }};

        try
        {
            var result = await GetDistanceMatrixResult(parameters);
            return $"{HtmlDecoration.Bold("From:")} {HtmlDecoration.Italic(result.Origin)}\n" +
                   $"{HtmlDecoration.Bold("To:")} {HtmlDecoration.Italic(result.Destination)}\n" +
                   $"{HtmlDecoration.Bold("Distance:")} {HtmlDecoration.Italic(result.Distance)}\n" +
                   $"{HtmlDecoration.Bold("Duration:")} {HtmlDecoration.Italic(result.Duration)}\n\n" +
                   $"{HtmlDecoration.Bold("Source:")} {HtmlDecoration.Underline("Google Maps")}©️";
        }
        catch
        {
            return "Please enter valid addresses";
        }
    }

    private async Task<DistanceMatrixResult> GetDistanceMatrixResult(Dictionary<string, string> parameters)
    {
        parameters.Add("units", "imperial");
        
        var data = await GetResultAsync("distancematrix/json", parameters);
        
        return new DistanceMatrixResult(){
                Origin = data["origin_addresses"][0].ToString(),
                Destination = data["destination_addresses"][0].ToString(),
                Distance = data["rows"][0]["elements"][0]["distance"]["text"].ToString(),
                Duration = data["rows"][0]["elements"][0]["duration"]["text"].ToString()};
    }

    private async Task<JObject> GetResultAsync(string resource, Dictionary<string, string> parameters = null)
    {
        var request = CreateNewRestRequest(resource, parameters);
        var response = await _client.GetAsync(request);
        if (response.IsSuccessful && response.Content is not null)
            return JObject.Parse(response.Content);
        throw new Exception("Error occured while trying to get a result.");
    }

    private static RestRequest CreateNewRestRequest(string resource, Dictionary<string, string> parameters)
    {
        var request = new RestRequest(resource);

        if (parameters is not null)
            foreach (var parameter in parameters)
                request.AddParameter(parameter.Key, parameter.Value);
        return request;
    }

    public async Task<object> GetGeocodingAsync(string location, bool reverse)
    {
        var parameters = new Dictionary<string, string>{{ reverse ? "address" : "latlng", location }};
        return await this.GetResultAsync("geocode/json", parameters);
    }

    public async Task<string> GetReverseGeocodingAsync(string coordinates)
    {
        var parameters = new Dictionary<string, string> {{ "latlng", coordinates }};
        var response = await this.GetResultAsync("geocode/json", parameters);
        var formattedAddress = response["results"][0]["formatted_address"].ToString();
        return formattedAddress;
    }

    public async Task<string> GetStaticMapAsync(string coordinates)
    {
        var parameters = GetStaticMapsDefaultParameters();
        parameters.Add("center", coordinates);
        parameters.Add("markers", coordinates);
        return await Task.FromResult(_client.BuildUri(CreateNewRestRequest("staticmap", parameters)).ToString());
    }

    public async Task<string> GetStaticMapAsync(string center, string[] objects)
    {
        var parameters = GetStaticMapsDefaultParameters();
        parameters.Add("center", center);

        var url = _client.BuildUri(CreateNewRestRequest("staticmap", parameters)).ToString();
        var colors = new[] { "red", "blue", "green", "yellow", "orange", "black", "white", "pink", "gray", "purple", "brown"};
        
        var sn = 0;
        foreach (string obj in objects)
            url += $"&markers=color:{colors[sn % colors.Length]}%7Clabel:{++sn}%7C{obj}";

        return await Task.FromResult(url);
    }

    private static Dictionary<string, string> GetStaticMapsDefaultParameters()
    {
        return new Dictionary<string, string>
           {{ "scale", "2" },
            { "language", "en" },
            { "zoom", "17" },
            { "maptype", "hybrid" },
            { "size", "700x700" },
            { "format", "jpg" }};
    }
}