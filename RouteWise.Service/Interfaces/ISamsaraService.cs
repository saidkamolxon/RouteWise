using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Interfaces;

public interface ISamsaraService
{
    Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default);
}
