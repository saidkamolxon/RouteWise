using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Helpers;

public class TruckAddressResolver : IValueResolver<JToken, TruckStateDto, Address>
{
    public Address Resolve(JToken source, TruckStateDto destination, Address destMember, ResolutionContext context)
    {
        // From samsara
        var addressArray = source["gps"]["reverseGeo"]["formattedLocation"].ToString().Split(',');
        return new Address
        {
            Street = addressArray.ElementAt(0).Trim(),
            City = addressArray.ElementAt(1).Trim(),
            State = addressArray.ElementAt(2).Trim(),
            ZipCode = addressArray.ElementAt(3).Trim()
        };
    }
}
