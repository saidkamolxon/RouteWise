using RouteWise.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RouteWise.Domain.Entities;

public class Landmark : Auditable
{
    public string Name { get; set; }
    public string BorderPoints { get; set; }

    [Required]
    public Address Address { get; set; }
}
