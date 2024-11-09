using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Truck;
using System.Globalization;
using System.Net;

namespace RouteWise.Service.Helpers;

public class TruckCoordinatesResolver : IValueResolver<JToken, TruckStateDto, Coordination>
{
    public Coordination Resolve(JToken source, TruckStateDto destination, Coordination destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Value<string>("gps")))
        {
            // From SwiftEld
            return new Coordination
            {
                Latitude = double.Parse(source.Value<string>("lat"), CultureInfo.InvariantCulture),
                Longitude = double.Parse(source.Value<string>("lng"), CultureInfo.InvariantCulture)
            };
        }

        var gps = source["gps"];
        
        // From Samsara
        return new Coordination
        {
            Latitude = gps.Value<double>("latitude"),
            Longitude = gps.Value<double>("longitude")
        };
    }
}
