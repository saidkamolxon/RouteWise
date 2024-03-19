using Newtonsoft.Json;
using RouteWise.Domain.Entities;
using RouteWise.Service.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RouteWise.Service.Services.FleetLocate;

public class FleetLocateService : IFleetLocateService
{
    private HttpClient _client;
    private int _tries;

    public FleetLocateService(FleetLocateAPICredentials credentials)
    {
        _tries = 10;
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.us.spireon.com/api/");
        
        Authorize(credentials);
    }

    private void Authorize(FleetLocateAPICredentials credentials)
    {
        string authString = this.GetAuthString(credentials.Login, credentials.Password);
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {authString}");
        _client.DefaultRequestHeaders.Add("Account", credentials.AccountId);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private string GetAuthString(string login, string password)
    {
        byte[] userBytes = Encoding.ASCII.GetBytes($"{login}:{password}");
        return Convert.ToBase64String(userBytes);
    }

    public async Task<object> GetAssetsAsync()
        => await this.GetDataAsync(url:"asset");

    public async Task<dynamic> GetAssetsStatusesAsync()
        => await this.GetDataAsync(url: "assetStatus");

    public async Task<object> GetLandmarksAsync()
        => await this.GetDataAsync(url: "landmark");

    public async Task<dynamic> GetLandmarksStatusesAsync()
        => await this.GetDataAsync(url: "landmarkStatus");

    #region Encapsulated methods --->>
    private async Task<dynamic> GetDataAsync(string url, string param = "data")
    {
        int tries = _tries;
        while (tries > 0)
        {
            dynamic jsonResponse = await GetJsonResponseAsync(url);
            if ((bool)jsonResponse.success)
                return jsonResponse[param];
            await Task.Delay(1000);
            tries--;
        }
        return -1;
    }

    private async Task<dynamic> GetJsonResponseAsync(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<dynamic>(responseBody);
    }
    #endregion
}