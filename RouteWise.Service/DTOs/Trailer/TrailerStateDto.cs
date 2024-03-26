using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;

namespace RouteWise.Service.DTOs.Trailer;

public class TrailerStateDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Coordinates { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventDate { get; set; }
    public int LandmarkId { get; set; }
}
