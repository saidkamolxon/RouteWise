using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Helpers;

public class TrailerCoordinatesResolver : IValueResolver<JToken, TrailerStateDto, Coordination>
{
    public Coordination Resolve(JToken source, TrailerStateDto destination, Coordination destMember, ResolutionContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(source.Value<string>("latitude")))
                return new Coordination
                    { Latitude = source.Value<double>("lat"), Longitude = source.Value<double>("lng") };
            return new Coordination
                { Latitude = source.Value<double>("latitude"), Longitude = source.Value<double>("longitude") };
        }
        catch
        {
            return null;
        }
    }
}
