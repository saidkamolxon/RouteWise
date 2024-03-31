using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Helpers;

public class TrailerAddressResolver : IValueResolver<JToken, TrailerStateDto, Address>
{
    public Address Resolve(JToken source, TrailerStateDto destination, Address destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Value<string>("location")))
            return new Address
            {
                Street = source.Value<string>("address"),
                City = source.Value<string>("city"),
                State = source.Value<string>("state"),
                ZipCode = source.Value<string>("zip"),
            };
        else
        {
            var addressArray = source.Value<string>("location").Split(',');
            if (addressArray.Length < 3)
                Console.WriteLine($"this is the thing: {source.Value<string>("trailerName")}");
            return new Address
            {
                Street = addressArray[0].Trim(),
                City = addressArray[1].Trim(),
                State = addressArray[2].Trim()[..2],
                ZipCode = addressArray[2].Trim()[3..],
            };
        }
    }
}
