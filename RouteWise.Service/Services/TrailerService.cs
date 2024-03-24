using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Exceptions;
using RouteWise.Service.Interfaces;
using System.Data.Common;

namespace RouteWise.Service.Services;

public class TrailerService : ITrailerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFleetLocateService _fleetLocateService;
    private readonly IRoadReadyService _roadReadyService;

    public TrailerService(IUnitOfWork unitOfWork, IMapper mapper,
                          IFleetLocateService fleetLocateService,
                          IRoadReadyService roadReadyService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fleetLocateService = fleetLocateService;
        _roadReadyService = roadReadyService;
    }

    public async Task<TrailerResultDto> CreateAsync(TrailerCreationDto dto)
    {
        var newTrailer = _mapper.Map<Trailer>(dto);
        await _unitOfWork.TrailerRepository.CreateAsync(newTrailer);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<TrailerResultDto>(newTrailer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(id)
             ?? throw new NotFoundException("Trailer with such id is not found.");
        
        _unitOfWork.TrailerRepository.Delete(trailer);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IReadOnlyList<TrailerResultDto>> GetAllAsync()
    {
        var trailers = await _unitOfWork.TrailerRepository
                                .SelectAll(asNoTracked: true)
                                .OrderBy(t => t.Name)
                                .ToListAsync();
        return _mapper.Map<IReadOnlyList<TrailerResultDto>>(trailers);
    }

    public async Task<TrailerResultDto> GetByIdAsync(int id)
    {
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(id)
            ?? throw new NotFoundException("Trailer with such id is not found.");

        return _mapper.Map<TrailerResultDto>(trailer);
    }

    public async Task<TrailerResultDto> GetByNameAsync(string name)
    {
        var trailer = await _unitOfWork.TrailerRepository
            .SelectAsync(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new NotFoundException("Trailer with such name is not found.");

        return _mapper.Map<TrailerResultDto>(trailer);
    }

    public async Task<TrailerResultDto> UpdateAsync(TrailerStateDto dto)
    {
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(dto.Id);
        var mappedTrailer = _mapper.Map(dto, trailer);

        _unitOfWork.TrailerRepository.Update(mappedTrailer);
        await _unitOfWork.SaveAsync();

        var updatedTrailer = await _unitOfWork.TrailerRepository.SelectAsync(mappedTrailer.Id);
        return _mapper.Map<TrailerResultDto>(updatedTrailer);
    }

    public async Task UpdateTrailersStatesAsync()
    {
        await _roadReadyService.GetTrailersStatesAsync();
        await _fleetLocateService.GetTrailersStates();
    }
}
