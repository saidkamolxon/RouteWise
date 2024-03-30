using RouteWise.Domain.Models;

namespace RouteWise.Service.DTOs.Landmark;

public class LandmarkUpdateDto
{
    public string Name { get; set; }
    public Coordinate Coordinates { get; set; }
    public IEnumerable<Coordinate> BorderPoints { get; set; }
    public Address Address { get; set; }
}