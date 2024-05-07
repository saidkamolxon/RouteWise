using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Interfaces;

public interface ITrailerService
{
    Task<TrailerResultDto> CreateAsync(TrailerCreationDto dto, CancellationToken cancellationToken = default);
    Task<TrailerResultDto> UpdateAsync(TrailerStateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<TrailerResultDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TrailerResultDto> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrailerResultDto>> GetByCityAndStateAsync(string city = null, string state = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrailerResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateTrailersStatesAsync(CancellationToken cancellationToken = default);
}
