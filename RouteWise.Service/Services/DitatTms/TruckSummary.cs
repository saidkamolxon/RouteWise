namespace RouteWise.Service.Services.DitatTms;

public record TruckSummary : IComparable<TruckSummary>
{
    public string Driver { get; set; }
    public string City { get;  set; }
    public string State { get; set; }
    public DateTime Time { get; set; }

    public int CompareTo(TruckSummary other)
    {
        if (State.Equals(other.State))
            return City.CompareTo(other.City);
        return State.CompareTo(other.State);
    }
}