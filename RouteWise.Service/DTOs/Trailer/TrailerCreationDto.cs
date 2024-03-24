namespace RouteWise.Service.DTOs.Trailer;

public class TrailerCreationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Vin { get; set; }
    public int Year { get; set; }
    public string License { get; set; }
    public DateOnly LastInspectionOn { get; set; }
    public DateOnly NextInspectionOn { get; set; }
}
