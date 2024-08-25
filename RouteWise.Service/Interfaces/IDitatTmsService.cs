using RouteWise.Domain.Enums;

namespace RouteWise.Service.Interfaces;

public interface IDitatTmsService
{
    Task<string> GetAvailableTrucksAsync(bool withDrivers = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Uri>> GetUnitDocumentsAsync(string unitId, UnitType unit, CancellationToken cancellationToken = default);
}
