using RouteWise.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RouteWise.Domain.Entities;

public class Landmark : Auditable
{
    [Required]
    public string Name { get; set; }

    [Required]
    public Coordination Coordinates { get; set; }

    [NotMapped]
    public IEnumerable<Coordination> BorderPoints { get; set; }

    [Required]
    public Address Address { get; set; }
    public IEnumerable<Trailer> Trailers { get; set; }
    public IEnumerable<Truck> Trucks { get; set; }

    public string BorderPointsJson
    {
        get => JsonSerializer.Serialize(BorderPoints);
        set => BorderPoints = JsonSerializer.Deserialize<IEnumerable<Coordination>>(value);
    }

    public override string ToString() => string.Format("{0} -> {1}", Name, Address);
}
