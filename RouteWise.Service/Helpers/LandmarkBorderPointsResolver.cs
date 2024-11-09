using System.Globalization;
using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Landmark;

namespace RouteWise.Service.Helpers;

public class LandmarkBorderPointsResolver : IValueResolver<JToken, LandmarkUpdateDto, IEnumerable<Coordination>>
{
    public IEnumerable<Coordination> Resolve(JToken source, LandmarkUpdateDto destination, IEnumerable<Coordination> destMember, ResolutionContext context)
    {
        var points = source.Value<string>("points");
        return Array.ConvertAll(points.Split(','), border =>
        {
            var xy = border.Split();
            return new Coordination { Latitude = double.Parse(xy[0], CultureInfo.InvariantCulture), Longitude = double.Parse(xy[1], CultureInfo.InvariantCulture) };
        })
        .ToList();
    }
}