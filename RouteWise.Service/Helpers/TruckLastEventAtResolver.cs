using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Helpers;

public class TruckLastEventAtResolver : IValueResolver<JToken, TruckStateDto, DateTime>
{
    public DateTime Resolve(JToken source, TruckStateDto destination, DateTime destMember, ResolutionContext context)
    {
        if (source.SelectToken("gps") is null)
        {
            // From SwiftEld
            var unixTime = source.Value<long>("signalTime");
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            return dateTimeOffset.DateTime;
        }

        // From Samsara
        return source["gps"].Value<DateTime>();
    }
}
