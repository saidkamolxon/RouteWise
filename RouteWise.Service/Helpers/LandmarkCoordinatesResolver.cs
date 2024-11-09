using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Landmark;

namespace RouteWise.Service.Helpers;

public class LandmarkCoordinatesResolver : IValueResolver<JToken, LandmarkUpdateDto, Coordination>
{
    public Coordination Resolve(JToken source, LandmarkUpdateDto destination, Coordination destMember, ResolutionContext context) 
        => new () { Latitude = source.Value<double>("lat"), Longitude = source.Value<double>("lng") };
}
