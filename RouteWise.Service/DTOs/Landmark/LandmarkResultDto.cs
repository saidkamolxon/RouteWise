﻿using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Helpers;

namespace RouteWise.Service.DTOs.Landmark;

public class LandmarkResultDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Coordinates { get; set; }
    public string PhotoUrl { get; set; }
    public IEnumerable<TrailerWithinLandmarkDto> Trailers { get; set; }
    public override string ToString()
    {
        var result = $"{HtmlDecoration.Bold(Name)} - {Address}";
        if (Trailers.Any()) result += HtmlDecoration.Bold("\n\nTRAILERS:\n");

        int sn = 1;
        foreach(var trailer in Trailers.OrderBy(t => t.ArrivedAt))
        {
            var duration = TimeHelper.ConvertUtcToDefaultTime(DateTime.UtcNow) - trailer.ArrivedAt;
            result += $"{HtmlDecoration.Bold((sn++).ToString())}. {HtmlDecoration.Code(trailer.Name.PadRight(10))} ➜ {HtmlDecoration.Code(trailer.Coordinates)} ({TimeHelper.FormatDuration(duration)})\n";
        }

        return result;
    }
}