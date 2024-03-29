using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Interfaces;

public interface ITrailerService
{
    Task<TrailerResultDto> CreateAsync(TrailerCreationDto dto);
    Task<TrailerResultDto> UpdateAsync(TrailerStateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<TrailerResultDto> GetByIdAsync(int id);
    Task<TrailerResultDto> GetByNameAsync(string name);
    Task<IReadOnlyList<TrailerResultDto>> GetAllAsync();
    Task UpdateTrailersStatesAsync();
}
