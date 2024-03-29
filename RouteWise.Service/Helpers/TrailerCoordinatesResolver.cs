using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Helpers;

public class TrailerCoordinatesResolver : IValueResolver<JToken, TrailerStateDto, string>
{
    public string Resolve(JToken source, TrailerStateDto destination, string destMember, ResolutionContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(source.Value<string>("latitude")))
            {
                return source.Value<string>("lat").Replace(',', '.') + "," +
                       source.Value<string>("lng").Replace(',', '.');
            }
            return source.Value<string>("latitude").Replace(',', '.') + "," +
                   source.Value<string>("longitude").Replace(',', '.');
        }
        catch
        {
            return string.Empty;
        }
    }
}
