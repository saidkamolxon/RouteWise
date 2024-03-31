using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.DTOs.Landmark;

public class LandmarkResultDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Coordinates { get; set; }
    public IEnumerable<TrailerWithinLandmarkDto> Trailers { get; set; }
}