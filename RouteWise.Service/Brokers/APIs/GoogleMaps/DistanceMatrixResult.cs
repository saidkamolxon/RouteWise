namespace RouteWise.Service.Brokers.APIs.GoogleMaps;

public record DistanceMatrixResult
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Distance { get; set; }
    public string Duration { get; set; }
}
