using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Landmark;

namespace RouteWise.Service.Helpers;

public class LandmarkAddressResolver : IValueResolver<JToken, LandmarkUpdateDto, Address>
{
    public Address Resolve(JToken source, LandmarkUpdateDto destination, Address destMember, ResolutionContext context)
    {
        return new Address
        {
            Street = source.Value<string>("address"),
            City = source.Value<string>("city"),
            State = source.Value<string>("state"),
            ZipCode = source.Value<string>("zip"),
        };
    }
}