using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Brokers.APIs.SwiftEld;

public interface ISwiftEldApiBroker
{
    Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync(CancellationToken cancellationToken = default);
    Task<TruckStateDto> GetTruckStateByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAllTruckNumbersAsync(CancellationToken cancellationToken = default);
}
