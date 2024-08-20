using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Driver : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int SwiftEldId { get; set; }
    
    [Phone]
    public string Phone { get; set; }
}
