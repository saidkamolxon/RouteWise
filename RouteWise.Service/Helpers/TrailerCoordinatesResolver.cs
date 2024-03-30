using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Helpers;

public class TrailerCoordinatesResolver : IValueResolver<JToken, TrailerStateDto, Coordinate>
{
    public Coordinate Resolve(JToken source, TrailerStateDto destination, Coordinate destMember, ResolutionContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(source.Value<string>("latitude")))
                return new Coordinate
                    { Latitude = source.Value<double>("lat"), Longitude = source.Value<double>("lng") };
            return new Coordinate
                { Latitude = source.Value<double>("latitude"), Longitude = source.Value<double>("longitude") };
        }
        catch
        {
            return null;
        }
    }
}
