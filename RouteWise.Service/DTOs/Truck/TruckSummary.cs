namespace RouteWise.Service.Brokers.APIs.DitatTms;

public record TruckSummaryDto : IComparable<TruckSummaryDto>
{
    public string Driver { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public DateTime Time { get; set; }

    public int CompareTo(TruckSummaryDto other)
    {
        if (State.Equals(other.State))
            return City.CompareTo(other.City);
        return State.CompareTo(other.State);
    }
}