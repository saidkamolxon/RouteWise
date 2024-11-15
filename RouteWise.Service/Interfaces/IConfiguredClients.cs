﻿using RestSharp;

namespace RouteWise.Service.Interfaces;

public interface IConfiguredClients
{
    IRestClient BingMapsClient { get; }
    IRestClient DitatTmsClient { get; }
    IRestClient FleetLocateClient { get; }
    IRestClient GoogleMapsClient { get; }
    IRestClient RoadReadyClient { get; }
    IRestClient SamsaraClient { get; }
    IRestClient SwiftEldClient { get; }
}
