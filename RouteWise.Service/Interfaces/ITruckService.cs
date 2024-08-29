using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Interfaces;

public interface ITruckService
{
    Task<TruckResultDto> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetTruckNumbersAsync(CancellationToken cancellationToken = default);
}