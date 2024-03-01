using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Driver : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int DriverId { get; set; } // SwiftELD Driver Id
    
    [Phone]
    public string Phone { get; set; }
}
