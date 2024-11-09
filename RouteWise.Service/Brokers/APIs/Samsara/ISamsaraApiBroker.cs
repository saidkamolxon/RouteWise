using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Brokers.APIs.Samsara;

public interface ISamsaraApiBroker
{
    Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<ICollection<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default);
    Task<string> GetDriverByTruckNameAsync(string truck, CancellationToken cancellationToken = default);
    Task<string> GetDriverByVehicleIdAsync(string vehicleId, CancellationToken cancellationToken = default);
    Task<TruckResultDto> GetVehicleByIdAsync(string vehicleId, CancellationToken cancellationToken = default);
}
