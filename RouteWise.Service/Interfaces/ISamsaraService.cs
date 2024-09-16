using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Interfaces;

public interface ISamsaraService
{
    Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default);
    Task<string> GetDriverByTruckNameAsync(string truck, CancellationToken cancellationToken = default);
    Task<string> GetDriverByVehicleIdAsync(string vehicleId, CancellationToken cancellationToken = default);
    Task<TruckResultDto> GetVehicleById(string vehicleId, CancellationToken cancellationToken = default);
}
