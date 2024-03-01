namespace RouteWise.Domain.Entities;

public class Landmark : Auditable
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Zip { get; set; } = default!;
    public string Points { get; set; } = default!;
}
