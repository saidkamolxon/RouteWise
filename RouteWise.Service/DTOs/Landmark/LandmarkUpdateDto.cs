using RouteWise.Domain.Models;

namespace RouteWise.Service.DTOs.Landmark;

public class LandmarkUpdateDto
{
    public string Name { get; set; }
    public Coordination Coordinates { get; set; }
    public IEnumerable<Coordination> BorderPoints { get; set; }
    public Address Address { get; set; }
}