namespace RouteWise.Domain.Entities;

public class State : Auditable
{
    public long ChatId { get; set; }
    public long UserId { get; set; }
    public string SerializedState { get; set; }
    public string DistanceOrigin { get; set; }
    public string DistanceDestination { get; set; }
}