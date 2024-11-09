using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RouteWise.Service.Brokers.APIs.DitatTms;
using RouteWise.Service.Brokers.APIs.FleetLocate;
using RouteWise.Service.Brokers.APIs.GoogleMaps;
using RouteWise.Service.Brokers.APIs.RoadReady;
using RouteWise.Service.Brokers.APIs.Samsara;
using RouteWise.Service.Brokers.APIs.SwiftEld;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Helpers;

public class ConfiguredClients(IConfiguration configuration, IMemoryCache cache) : IConfiguredClients, IDisposable
{
    private readonly IConfiguration configuration = configuration;
    private readonly IMemoryCache cache = cache;

    public IRestClient DitatTmsClient => ditatTmsClient.Value;
    public IRestClient GoogleMapsClient => googleMapsClient.Value;
    public IRestClient FleetLocateClient => fleetlocateClient.Value;
    public IRestClient RoadReadyClient => roadReadyClient.Value;
    public IRestClient SamsaraClient => samsaraClient.Value;
    public IRestClient SwiftEldClient => swiftEldClient.Value;

    private Lazy<IRestClient> ditatTmsClient => new(() =>
    {
        var credentials = this.configuration.GetSection("AccessToExternalAPIs:DitatTMS").Get<DitatTmsApiCredentials>();
        var client = new RestClient(credentials.BaseUrl);
        ditatTmsEnsureAuthenticated(client, credentials);
        return client;
    });

    #region DitatTMS Auth Logic
    
    private void ditatTmsEnsureAuthenticated(IRestClient client, DitatTmsApiCredentials credentials)
    {
        if (!this.cache.TryGetValue("Ditat-token", out string token))
        {
            ditatTmsAuthenticate(client, credentials);
        }
        else
        {
            ditatTmsAuthorize(client, token);
        }
    }

    private void ditatTmsAuthenticate(IRestClient client, DitatTmsApiCredentials credentials)
    {
        var authString = AuthorizationHelper.GetAuthString(credentials.Username, credentials.Password);
        var request = new RestRequest("auth/login")
            .AddHeader("ditat-account-id", credentials.AccountId)
            .AddHeader("ditat-application-role", credentials.ApplicationRole)
            .AddHeader("Authorization", $"Basic {authString}");
        
        var response = client.Post(request);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            this.cache.Set("Ditat-token", response.Content, TimeSpan.FromHours(4));
            ditatTmsAuthorize(client, response.Content);
            return;
        }
        throw response.ErrorException;
    }

    private void ditatTmsAuthorize(IRestClient client, string token)
        => client.AddDefaultHeader("Authorization", $"Ditat-token {token}");

    #endregion

    private Lazy<IRestClient> fleetlocateClient => new(() =>
    {
        var credentials = this.configuration.GetSection("AccessToExternalAPIs:FleetLocate").Get<FleetLocateApiCredentials>();
        string authString = AuthorizationHelper.GetAuthString(credentials.Login, credentials.Password);
        var client = new RestClient(credentials.BaseUrl);
        client.AddDefaultHeader("Authorization", $"Basic {authString}");
        client.AddDefaultHeader("Account", credentials.AccountId);
        client.AddDefaultHeader("accept", "application/json");
        return client;
    });

    private Lazy<IRestClient> googleMapsClient => new(() =>
    {
        var credentials = this.configuration.GetSection("AccessToExternalAPIs:GoogleMaps").Get<GoogleMapsApiCredentials>();
        return new RestClient(credentials.BaseUrl).AddDefaultParameter("key", credentials.Token);
    });

    private Lazy<IRestClient> roadReadyClient => new(() =>
    {
        var credentials = this.configuration.GetSection("AccessToExternalAPIs:RoadReady").Get<RoadReadyApiCredentials>();
        return new RestClient(credentials.BaseUrl).AddDefaultHeader("x-api-key", credentials.Token);
    });

    private Lazy<IRestClient> samsaraClient => new(() =>
    {
        var credentials = this.configuration.GetSection("AccessToExternalAPIs:Samsara").Get<SamsaraApiCredentials>();
        return new RestClient(credentials.BaseUrl).AddDefaultHeader("Authorization", $"Bearer {credentials.Token}");
    });

    private Lazy<IRestClient> swiftEldClient => new(() =>
    {
        var credentials = this.configuration.GetSection("AccessToExternalAPIs:SwiftELD").Get<SwiftEldApiCredentials>();
        return new RestClient(credentials.BaseUrl).AddDefaultParameter("token", credentials.Token);
    });

    public void Dispose() => GC.SuppressFinalize(this);
}
