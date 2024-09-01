using RouteWise.Domain.Models;
using RouteWise.Service.Helpers;
using System.Globalization;

namespace RouteWise.Service.DTOs.Truck;

public class TruckResultDto
{
    public string Name { get; set; }
    public string License { get; set; }
    public string Vin { get; set; }
    public string Address { get; set; }
    public DateTime LastEventAt { get; set; }
    public Coordinate Coordinates { get; set; }
    public string Odometer { get; set; }
    public int? DriverId { get; set; }
    public string Speed { get; set; }
    public string PhotoUrl { get; set; }

    public override string ToString()
    {
        string lastEventAt = LastEventAt.ToString("dd-MMM HH:mm", new CultureInfo("en-US"));
        string movingSymbol = Speed.StartsWith('0') ? "🔴" : "🟢";
        return $"🚚 {HtmlDecoration.Bold(Name)} {Speed} {movingSymbol}\n\n" +
               $"Coordinates: {HtmlDecoration.Code(Coordinates.ToString())}\n" +
               $"Location: {HtmlDecoration.Bold(Address)}\n\n" +
               $"👉 {HtmlDecoration.Bold(HtmlDecoration.Link("LINK", "https://maps.google.com/maps?q=" + Coordinates))} 👈\n" +
               HtmlDecoration.Spoiler(HtmlDecoration.Italic($"Last GPS update: {lastEventAt} EST")); ;
    }
}
