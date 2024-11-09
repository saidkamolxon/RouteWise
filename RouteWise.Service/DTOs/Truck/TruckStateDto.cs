using RouteWise.Domain.Models;

namespace RouteWise.Service.DTOs.Truck;

public class TruckStateDto
{
    public string Name { get; set; }
    public string License { get; set; }
    public string Vin { get; set; }
    public string Address { get; set; }
    public DateTime LastEventAt { get; set; }
    public Coordination Coordinates { get; set; }
    public string Odometer { get; set; }
    public int? DriverId { get; set; }
    public string Speed { get; set; }
}
