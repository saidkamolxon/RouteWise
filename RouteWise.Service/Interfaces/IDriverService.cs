using RouteWise.Service.DTOs.Driver;

namespace RouteWise.Service.Interfaces;

public interface IDriverService
{
    Task<IEnumerable<DriverResultDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}