using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Truck;
using System.Globalization;

namespace RouteWise.Service.Helpers;

public class TruckCoordinatesResolver : IValueResolver<JToken, TruckStateDto, Coordinate>
{
    public Coordinate Resolve(JToken source, TruckStateDto destination, Coordinate destMember, ResolutionContext context)
    {
        return new Coordinate
        {
            Latitude = double.Parse(source.Value<string>("lat"), CultureInfo.InvariantCulture),
            Longitude = double.Parse(source.Value<string>("lng"), CultureInfo.InvariantCulture)
        };
    }
}
