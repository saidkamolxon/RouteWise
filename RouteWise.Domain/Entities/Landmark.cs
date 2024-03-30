using Newtonsoft.Json;
using RouteWise.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RouteWise.Domain.Entities;

public class Landmark : Auditable
{
    [Required]
    public string Name { get; set; }

    [Required]
    public Coordinate Coordinates { get; set; }

    [NotMapped]
    public IEnumerable<Coordinate> BorderPoints { get; set; }

    [Required]
    public Address Address { get; set; }
    public IEnumerable<Trailer> Trailers { get; set; }

    public string BorderPointsJson
    {
        get => JsonConvert.SerializeObject(BorderPoints);
        set => BorderPoints = JsonConvert.DeserializeObject<IEnumerable<Coordinate>>(value);
    }

    public override string ToString()
    {
        return Name;
    }
}
