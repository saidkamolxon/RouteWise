using RouteWise.Domain.Enums;

namespace RouteWise.Service.Brokers.APIs.DitatTms;

public interface IDitatTmsApiBroker
{
    Task<string> GetTrucksStateWhichHasLoadsAsync(CancellationToken cancellationToken = default);
    Task<string> GetAvailableTrucksAsync(bool withDrivers = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Uri>> GetUnitDocumentsAsync(string unitId, UnitType unitType, CancellationToken cancellationToken = default);
}
