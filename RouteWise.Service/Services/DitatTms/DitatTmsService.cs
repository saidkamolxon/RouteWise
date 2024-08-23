using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.Interfaces;
using System.Net;
using System.Text;

namespace RouteWise.Service.Services.DitatTms;

public class DitatTmsService : IDitatTmsService
{
    private readonly RestClient _client;
    private readonly IMapper _mapper;
    private readonly DitatTmsApiCredentials _credentials;
    private readonly IMemoryCache _cache;

    private const string _tokenCacheKey = "Ditat-token";

    public DitatTmsService(DitatTmsApiCredentials credentials, IMemoryCache cache)
    {
        _client = new RestClient(credentials.BaseUrl);
        _credentials = credentials;
        _cache = cache;
    }

    public async Task<string> GetAvailableTrucksAsync(bool withDrivers = true, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("planning-board")
            .AddParameter("truckSummaryUpdateCounter", 0)
            .AddParameter("pendingLoadSummaryUpdateCounter", 0)
            .AddParameter("truckDriverRealTimeUpdateCounter", 0);
        
        var data = await this.getDataAsync(request, cancellationToken: cancellationToken);

        var truckSummaries = data.Value<IEnumerable<dynamic>>("truckSummaries");
            
        var sortedSummaries = new List<TruckSummary>();
            
        foreach (var ts in truckSummaries)
        {
            try
            {
                sortedSummaries.Add(new()
                {
                    Driver = ts.primaryDriverFullName,
                    City = ts.readyAddress.municipality,
                    State = ts.readyAddress.administrativeArea,
                    Time = ts.readyOnLocal
                });
            }
            catch
            {
                continue;
            }
        }

        var builder = new StringBuilder();
        builder.AppendLine($"Truck list {DateTime.Today:MM/dd}");

        var currentState = "";
        sortedSummaries.Sort();
        foreach (var summary  in sortedSummaries)
        {
            if (summary.Time.Date > DateTime.Today.Date) continue;

            if (summary.State != currentState)
                builder.AppendLine();
            currentState = summary.State;
                
            builder.Append($"{summary.City}, {summary.State}");

            //try
            //{
            //    var driver = summary.Driver.Split();
            //    builder.Append($" - {driver[0]} {driver[1][0]}.");
            //}
            //catch
            //{
            //    continue;
            //}
            ////}
                
            if (summary.Time.Date == DateTime.Today.Date)
                builder.Append($" - {summary.Time:t}");

            builder.AppendLine();
        }
            
        var result = Convert.ToString(builder);
        return result;
    }

    public async Task<IEnumerable<Uri>> GetTrailerDocsAsync(string trailer, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("data/trailer")
            .AddParameter("id", trailer)
            .AddParameter("includeDocuments", true);

        var data = await this.getDataAsync(request, cancellationToken: cancellationToken);
        string trailerKey = data.Value<dynamic>("entityGraph").trailerKey;
            
        var documents = data.Value<IEnumerable<dynamic>>("documents");

        if (!documents.Any())
            return [];

        return documents
            .Select(d =>
                _client.BuildUri(
                    new RestRequest($"data/trailer/{trailerKey}/document/{d.documentKey}/file")
                        .AddParameter("ditat-token", _cache.Get<string>(_tokenCacheKey))));
    }

    private async Task<JObject> getDataAsync(RestRequest request, string param = "data", CancellationToken cancellationToken = default)
    {
        ensureAuthenticated();

        var response = await _client.GetAsync(request, cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            authenticate();
            response = await _client.GetAsync(request, cancellationToken);
        }

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JObject.Parse(response.Content).Value<JObject>(param);
        
        throw response.ErrorException;
    }

    #region Auth --->>>
    private void authenticate()
    {
        var request = new RestRequest("auth/login");
        request.AddHeader("ditat-account-id", _credentials.AccountId);
        request.AddHeader("ditat-application-role", _credentials.ApplicationRole);
        
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_credentials.Username}:{_credentials.Password}"));
        request.AddHeader("authorization", $"Basic {encoded}");
        
        var response = _client.Post(request);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var token = response.Content;
            var expiration = TimeSpan.FromHours(12);
            _cache.Set(_tokenCacheKey, token, expiration);
            authorize(token);
            return;
        }
        throw response.ErrorException;
    }

    private void ensureAuthenticated()
    {
        if (!_cache.TryGetValue(_tokenCacheKey, out string token))
        {
            authenticate();
        }
        else
        {
            authorize(token);
        }
    }

    private void authorize(string token)
        => _client.AddDefaultHeader("Authorization", $"Ditat-token {token}");
    #endregion
}
