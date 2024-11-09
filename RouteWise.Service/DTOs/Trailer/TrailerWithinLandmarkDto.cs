namespace RouteWise.Service.DTOs.Trailer;

public class TrailerWithinLandmarkDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Coordinates { get; set; }
    public bool IsMoving { get; set; }
    public DateTime LastEventAt { get; set; }
    public DateTime ArrivedAt { get; set; }
}
