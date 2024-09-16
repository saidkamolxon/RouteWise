using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Driver : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int SwiftEldId { get; set; }
    public string DitatId { get; set; }
    public string SamsaraId { get; set; }

    [Phone]
    public string Phone { get; set; }
}
