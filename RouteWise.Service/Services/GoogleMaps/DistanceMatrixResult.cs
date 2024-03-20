namespace RouteWise.Service.Services.GoogleMaps;

public class DistanceMatrixResult
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Distance { get; set; }
    public string Duration { get; set; }
}