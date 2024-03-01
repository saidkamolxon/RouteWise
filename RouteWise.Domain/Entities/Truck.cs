using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Truck : Auditable
{
    public string Name { get; set; } = default!;
    public string License { get; set; } = default!; // License Plate Number

    [MinLength(17), MaxLength(17)]
    public string VIN { get; set; } = default!; // Vehicle Identification Number 17 chars
    public string Color { get; set; } = default!;
    public int Year { get; set; } = default!;

    public int DriverId { get; set; }
}
