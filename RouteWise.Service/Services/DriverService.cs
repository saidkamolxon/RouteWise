using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Driver;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class DriverService(IRepository<Driver> repository, IMapper mapper) : IDriverService
{
    private readonly IRepository<Driver> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DriverResultDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var drivers = await _repository.SelectAll(d => d.DitatId.Contains(name))
                                       .ToListAsync(cancellationToken: cancellationToken);
        
        return _mapper.Map<IEnumerable<DriverResultDto>>(drivers);
    }
}