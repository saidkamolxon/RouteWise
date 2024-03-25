using RouteWise.Domain.Models;

namespace RouteWise.Domain.Entities;

public class Landmark : Auditable
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public string BorderPoints { get; set; }
}
