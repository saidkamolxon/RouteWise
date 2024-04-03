using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Interfaces;

public interface ITruckService
{
    Task<TruckResultDto> GetAsync(string name);
}