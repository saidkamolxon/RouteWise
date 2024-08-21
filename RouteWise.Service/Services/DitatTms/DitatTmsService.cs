using AutoMapper;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Service.Interfaces;
using System.Text;

namespace RouteWise.Service.Services.DitatTms;

public class DitatTmsService : IDitatTmsService
{
    private readonly RestClient _client;
    private readonly IMapper _mapper;

    public DitatTmsService(DitatTmsApiCredentials credentials)
    {
        _client = new RestClient(credentials.BaseUrl);
        _client.AddDefaultHeader("Authorization", credentials.Token);
    }

    public async Task<string> GetAvailableTrucksAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("planning-board");
        request.AddParameter("truckSummaryUpdateCounter", 0);
        request.AddParameter("pendingLoadSummaryUpdateCounter", 0);
        request.AddParameter("truckDriverRealTimeUpdateCounter", 0);
        var response = await _client.GetAsync(request, cancellationToken);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var data = JObject.Parse(response.Content)
                              .Value<JObject>("data");

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

            var city = "";
            sortedSummaries.Sort();
            foreach (var summary  in sortedSummaries)
            {
                if (summary.Time.Date > DateTime.Today.Date) continue;

                if (summary.City != city)
                    builder.AppendLine();

                city = summary.City;
                var driver = summary.Driver.Split();
                if (summary.Time.Date == DateTime.Today.Date)
                    builder.AppendLine($"{summary.City}, {summary.State} - {driver[0]} {driver[1][0]}. - {summary.Time:t}");
                else if (summary.Time.Date < DateTime.Today.Date)
                    builder.AppendLine($"{summary.City}, {summary.State} - {driver[0]} {driver[1][0]}.");
            }
            
            var result = Convert.ToString(builder);
            return result;
        }
        throw new Exception("A bad request from ditat tms");
    }

    private async Task<IEnumerable<JToken>> GetDataAsync(string source, string param = "data", CancellationToken cancellationToken = default)
    {
        var jsonResponse = await GetJsonResponseAsync(source, cancellationToken);
        return jsonResponse.Value<IEnumerable<JToken>>(param);
    }
    private async Task<JObject> GetJsonResponseAsync(string source, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(new RestRequest(source), cancellationToken);
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            return JObject.Parse(response.Content);
        }
        throw new Exception("A bad request...");
    }
}

public record TruckSummary : IComparable<TruckSummary>
{
    public string Driver { get; set; }
    public string City { get;  set; }
    public string State { get; set; }
    public DateTime Time { get; set; }

    public int CompareTo(TruckSummary other)
    {
        if (State.Equals(other.State))
            return City.CompareTo(other.City);
        return State.CompareTo(other.State);
    }
}