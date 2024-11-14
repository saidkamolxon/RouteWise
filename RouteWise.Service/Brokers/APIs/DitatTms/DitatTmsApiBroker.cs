using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using RestSharp;
using RouteWise.Domain.Enums;
using RouteWise.Service.Brokers.APIs.SwiftEld;
using RouteWise.Service.Extensions;
using RouteWise.Service.Interfaces;
using System.Net;
using System.Text;

namespace RouteWise.Service.Brokers.APIs.DitatTms;

public class DitatTmsApiBroker : IDitatTmsApiBroker
{
    private readonly IMemoryCache _cache;
    private readonly IRestClient _client;
    private readonly ISwiftEldApiBroker _swiftEldService;
    private readonly IMapper _mapper;

    public DitatTmsApiBroker(IMemoryCache cache, IConfiguredClients configuredClients, ISwiftEldApiBroker swiftEldService)
    {
        _client = configuredClients.DitatTmsClient;
        _cache = cache;
    }

    public async Task<string> GetTrucksStateWhichHasLoadsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("dispatch-board", Method.Post).AddJsonBody(new { UpdateCounter = 0 });

        var response = await executeRequestAndGetDataAsync(request, cancellationToken: cancellationToken);

        var trips = response.Value<JArray>("trips");
        var trucks = await _swiftEldService.GetAllTrucksStatesAsync(cancellationToken);

        var builder = new StringBuilder();
        foreach (var trip in trips)
        {
            var truck = trip.Value<string>("truckId");
            var speed = "";
            try
            {
                speed = trucks.FirstOrDefault(t => t.Name.Equals(truck)).Speed;
            }
            catch
            {

            }
            var symbol = speed == "0 mph" ? "🔴" : "🟢";
            var driver = trip.Value<string>("primaryDriverId");
            //var nextAddress = $"{trip["toAddress"]["address1"]}, {trip["toAddress"]["municipality"]}, {trips["toAddress"]["administrativeArea"]}";
            builder.AppendLine($"<code>{speed.PadLeft(6)}</code>{symbol}<code>{truck.PadRight(6)}</code> {driver.Split().First().Capitalize().PadRight(10)} ➜ Left: 1064mi (15hrs 12mins)");
        }

        return builder.ToString();
    }

    public async Task<string> GetAvailableTrucksAsync(bool withDrivers = true, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("planning-board")
            .AddParameter("truckSummaryUpdateCounter", 0)
            .AddParameter("pendingLoadSummaryUpdateCounter", 0)
            .AddParameter("truckDriverRealTimeUpdateCounter", 0);

        var data = await executeRequestAndGetDataAsync(request, cancellationToken: cancellationToken);

        var truckSummaries = data.Value<IEnumerable<dynamic>>("truckSummaries");

        var sortedSummaries = new List<TruckSummaryDto>();

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
        foreach (var summary in sortedSummaries)
        {
            if (summary.Time.Date > DateTime.Today.Date) continue;

            if (summary.State != currentState)
                builder.AppendLine();
            currentState = summary.State;

            builder.Append($"{summary.City}, {summary.State}");

            try
            {
                if (withDrivers)
                {
                    var driver = summary.Driver.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    builder.Append($" - {driver[0].Capitalize()} {driver[1][0]}.");
                }
            }
            catch
            {
                continue;
            }

            if (summary.Time.Date == DateTime.Today.Date)
                builder.Append($" - {summary.Time:t}");

            builder.AppendLine();
        }

        var result = Convert.ToString(builder);
        return result;
    }

    public async Task<IEnumerable<Uri>> GetUnitDocumentsAsync(string unitId, UnitType unitType, CancellationToken cancellationToken = default)
    {
        string source = unitType switch
        {
            UnitType.Driver => "driver",
            UnitType.Truck => "truck",
            UnitType.Trailer => "trailer",
            _ => throw new Exception("UnitType is not selected.")
        };

        var request = new RestRequest($"data/{source}")
            .AddParameter("id", unitId)
            .AddParameter("includeDocuments", true);

        var data = await executeRequestAndGetDataAsync(request, cancellationToken: cancellationToken);
        string key = string.Format("{0}Key", source);
        string unitKey = data.Value<dynamic>("entityGraph")[key];

        var documents = data.Value<IEnumerable<dynamic>>("documents");

        if (!documents.Any())
            return [];

        return documents
            .Select(d =>
                _client.BuildUri(
                    new RestRequest($"data/{source}/{unitKey}/document/{d.documentKey}/file")
                        .AddParameter("ditat-token", _cache.Get<string>("Ditat-token"))));
    }

    private async Task<JObject> executeRequestAndGetDataAsync(RestRequest request, string param = "data", CancellationToken cancellationToken = default)
    {
        var response = await _client.ExecuteAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            response = await _client.ExecuteAsync(request, cancellationToken);
        }

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            return JObject.Parse(response.Content).Value<JObject>(param);

        throw response.ErrorException;
    }
}
