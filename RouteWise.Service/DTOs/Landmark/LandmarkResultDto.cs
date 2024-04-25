using RouteWise.Service.DTOs.Trailer;
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
        foreach(var trailer in Trailers)
            result += $"{HtmlDecoration.Bold((sn++).ToString())}. {HtmlDecoration.Code(trailer.Name):10f} ➜ {HtmlDecoration.Code(trailer.Coordinates)}\n";

        return result;
    }
}