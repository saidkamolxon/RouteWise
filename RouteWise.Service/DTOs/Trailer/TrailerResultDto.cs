using RouteWise.Service.Helpers;
using System.Globalization;

namespace RouteWise.Service.DTOs.Trailer;

public class TrailerResultDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Vin { get; set; }
    public int Year { get; set; }
    public string License { get; set; }
    public string Coordinates { get; set; }
    public string Address { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventAt { get; set; }
    public string PhotoUrl { get; set; }
    public string Landmark { get; set; }

    public override string ToString()
    {
        string lastEventAt = LastEventAt.ToString("dd-MMM HH:mm", new CultureInfo("en-US"));
        string movingSymbol = IsMoving ? "🔴" : "🟢";
        return $"Trailer#: {HtmlDecoration.Bold(Name)} {movingSymbol}\n\n" +
               $"Coordinates: {HtmlDecoration.Code(Coordinates)}\n" +
               $"Location: {HtmlDecoration.Bold(Address)}\n\n" +
               $"👉 {HtmlDecoration.Bold(HtmlDecoration.Link("LINK", "https://maps.google.com/maps?q=" + Coordinates))} 👈\n" +
               HtmlDecoration.Spoiler(HtmlDecoration.Italic($"Last GPS update: {lastEventAt} EST"));
    }
}
