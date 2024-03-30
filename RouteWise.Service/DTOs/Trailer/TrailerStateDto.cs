using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;

namespace RouteWise.Service.DTOs.Trailer;

public class TrailerStateDto
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public Coordinate Coordinates { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventAt { get; set; }
}
