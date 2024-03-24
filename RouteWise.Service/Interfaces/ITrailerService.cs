using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;
using RouteWise.Service.DTOs.Trailer;
using System.Linq.Expressions;

namespace RouteWise.Service.Interfaces;

public interface ITrailerService
{
    Task<TrailerResultDto> CreateAsync(TrailerCreationDto dto);
    Task<TrailerResultDto> UpdateAsync(TrailerStateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<TrailerResultDto> GetByIdAsync(int id);
    Task<TrailerResultDto> GetByNameAsync(string name);
    Task<IReadOnlyList<TrailerResultDto>> GetAllAsync();
}
