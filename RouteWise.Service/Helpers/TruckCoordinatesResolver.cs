using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Truck;
using System.Globalization;
using System.Net;

namespace RouteWise.Service.Helpers;

public class TruckCoordinatesResolver : IValueResolver<JToken, TruckStateDto, Coordinate>
{
    public Coordinate Resolve(JToken source, TruckStateDto destination, Coordinate destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Value<string>("gps")))
        {
            // From SwiftEld
            return new Coordinate
            {
                Latitude = double.Parse(source.Value<string>("lat"), CultureInfo.InvariantCulture),
                Longitude = double.Parse(source.Value<string>("lng"), CultureInfo.InvariantCulture)
            };
        }

        var gps = source["gps"];
        
        // From Samsara
        return new Coordinate
        {
            Latitude = gps.Value<double>("latitude"),
            Longitude = gps.Value<double>("longitude")
        };
    }
}
