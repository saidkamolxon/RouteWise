using RouteWise.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Trailer : Auditable
{
    public string Name { get; set; }
    public string Vin { get; set; }
    public int Year { get; set; }
    public string License { get; set; }
    public string Coordinates { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventAt { get; set; }
    public DateOnly LastInspectionOn { get; set; }
    public DateOnly NextInspectionOn { get; set; }

    [Required]
    public Address Address { get; set; }

    public int LandmarkId { get; set; }
    public Landmark Landmark { get; set; }
}
