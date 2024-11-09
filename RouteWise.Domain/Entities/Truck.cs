using RouteWise.Domain.Enums;
using RouteWise.Domain.Models;

namespace RouteWise.Domain.Entities;

public class Truck : Auditable
{
    public required string Name { get; set; }
    public Ownership Ownership { get; set; }
    public string License { get; set; }
    public string Vin { get; set; }
    public string Color { get; set; }
    public int? Year { get; set; }
    public required Address Address { get; set; }
    public DateTime LastEventAt { get; set; }
    public required Coordination Coordinates { get; set; }
    public long? Odometer { get; set; }
    //public int? DriverId { get; set; }
    //public Driver Driver { get; set; }
    //public int? CoDriverId { get; set; }
    //public Driver CoDriver { get; set; }
    public int? LandmarkId { get; set; }
    public Landmark Landmark { get; set; }
}
