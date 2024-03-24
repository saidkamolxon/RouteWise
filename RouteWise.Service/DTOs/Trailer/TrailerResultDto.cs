namespace RouteWise.Service.DTOs.Trailer;

public class TrailerResultDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Vin { get; set; }
    public int Year { get; set; }
    public string License { get; set; }
    public string Coordinates { get; set; }
    public string Address { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventAt { get; set; }

    public string Landmark { get; set; }
}