using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace RouteWise.Service.Services.FleetLocate;

public class FleetLocateService
{
    private string _user;
    private HttpClient _client;
    private int _tries;

    public FleetLocateService(string login, string password, string accountId)
    {
        _client = new HttpClient();
        _tries = 10;
        _user = $"{login}:{password}";

        string authString = this.GetAuthString();
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {authString}");
        _client.DefaultRequestHeaders.Add("Account", accountId);
        _client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        _client.BaseAddress = new Uri("https://api.us.spireon.com/api/");
    }

    public async Task<object> GetAssetsAsync()
        => await this.GetDataAsync(url:"asset", param:"data");
    
    public async Task<object> GetAssetsStatusesAsync()
        => await this.GetDataAsync(url:"assetStatus", param:"data");

    public async Task<object> GetLandmarksAsync()
        => await this.GetDataAsync(url:"landmark", param:"data");

    #region Encapsulated methods --->>
    private string GetAuthString()
    {
        byte[] userBytes = Encoding.ASCII.GetBytes(_user);
        string base64String = Convert.ToBase64String(userBytes);
        return base64String;
    }

    private async Task<object> GetDataAsync(string url, string param = "")
    {
        int tries = _tries;
        while (tries > 0)
        {
            var response = await _client.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
                if (!string.IsNullOrEmpty(param))
                    return jsonResponse[param];
                return jsonResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            await Task.Delay(1000);
            tries--;
        }
        return -1;
    }
    #endregion
}