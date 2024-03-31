using RouteWise.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Truck : Auditable
{
    [Required]
    public string Name { get; set; }
    public string License { get; set; }
    public string Vin { get; set; }
    public string Color { get; set; }
    public int? Year { get; set; }
    [Required]
    public Address Address { get; set; }
    public DateTime LastEventAt { get; set; }
    [Required]
    public Coordinate Coordinates { get; set; }
    public long? Odometer { get; set; }
    //public int? DriverId { get; set; }
    //public Driver Driver { get; set; }
    //public int? CoDriverId { get; set; }
    //public Driver CoDriver { get; set; }
    public int? LandmarkId { get; set; }
    public Landmark Landmark { get; set; }
}
