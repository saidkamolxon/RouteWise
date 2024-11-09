using RouteWise.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Trailer : Auditable
{
    [Required]
    public string Name { get; set; }
    [Required]
    public Coordination Coordinates { get; set; }
    [Required]
    public Address Address { get; set; }
    public string Vin { get; set; }
    public int? Year { get; set; }
    public string License { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventAt { get; set; }
    public DateOnly? LastInspectionOn { get; set; }
    public DateOnly? NextInspectionOn { get; set; }

    public int? LandmarkId { get; set; }
    public Landmark Landmark { get; set; }
    public DateTime? ArrivedAt { get; set; } // Arrived at the landmark at this time
}
