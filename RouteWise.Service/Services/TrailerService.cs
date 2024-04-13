using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Exceptions;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class TrailerService : ITrailerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFleetLocateService _fleetLocateService;
    private readonly IRoadReadyService _roadReadyService;
    private readonly ILandmarkService _landmarkService;
    private readonly IGoogleMapsService _googleMapsService;

    public TrailerService(IUnitOfWork unitOfWork, IMapper mapper,
                          IFleetLocateService fleetLocateService,
                          IRoadReadyService roadReadyService,
                          ILandmarkService landmarkService,
                          IGoogleMapsService googleMapsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fleetLocateService = fleetLocateService;
        _roadReadyService = roadReadyService;
        _landmarkService = landmarkService;
        _googleMapsService = googleMapsService;
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
                                .SelectAll(asNoTracking: true, includes: new [] { "Landmark" })
                                .OrderBy(t => t.Name)
                                .ToListAsync();
        return _mapper.Map<IReadOnlyList<TrailerResultDto>>(trailers);
    }

    public async Task<TrailerResultDto> GetByIdAsync(int id)
    { 
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(id, includes: new[] { "Landmark" })
            ?? throw new NotFoundException("Trailer with such id is not found.");

        return _mapper.Map<TrailerResultDto>(trailer);
    }

    public async Task<TrailerResultDto> GetByNameAsync(string name)
    {
        var trailer = await _unitOfWork.TrailerRepository
            .SelectAsync(t => t.Name.ToLower().Contains(name.ToLower()))
                ?? throw new NotFoundException("Trailer with such name is not found.");

        var dto = _mapper.Map<TrailerResultDto>(trailer);
        dto.PhotoUrl = await _googleMapsService.GetStaticMapAsync(dto.Coordinates);
        return dto;
    }

    public async Task<TrailerResultDto> UpdateAsync(TrailerStateDto dto)
    {
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(1);
        var mappedTrailer = _mapper.Map(dto, trailer);

        _unitOfWork.TrailerRepository.Update(mappedTrailer);
        await _unitOfWork.SaveAsync();

        var updatedTrailer = await _unitOfWork.TrailerRepository.SelectAsync(mappedTrailer.Id);
        return _mapper.Map<TrailerResultDto>(updatedTrailer);
    }

    public async Task UpdateTrailersStatesAsync()
    {
        var trailerStates = MergeData(await _fleetLocateService.GetTrailersStatesAsync(), 
                                      await _roadReadyService.GetTrailersStatesAsync());

        foreach (var state in trailerStates)
        {
            var trailer = await _unitOfWork.TrailerRepository.SelectAsync(t => t.Name.Equals(state.Name));
            if (trailer is not null)
            {
                _mapper.Map(state, trailer);
                trailer.LandmarkId = await _landmarkService.GetLandmarkIdOrDefaultAsync(trailer.Address.State, trailer.Coordinates);
                _unitOfWork.TrailerRepository.Update(trailer);
            }
            else
            {
                var newTrailer = _mapper.Map<Trailer>(state);
                newTrailer.LandmarkId = await _landmarkService.GetLandmarkIdOrDefaultAsync(newTrailer.Address.State, newTrailer.Coordinates);
                await _unitOfWork.TrailerRepository.CreateAsync(newTrailer);
            }
            await _unitOfWork.SaveAsync();
        }

        await Console.Out.WriteLineAsync("Done");
    }

    private static List<TrailerStateDto> MergeData(IEnumerable<TrailerStateDto> firstData, IEnumerable<TrailerStateDto> secondData)
    {
        return firstData
            .Concat(secondData)
            .GroupBy(trl => trl.Name)
            .Select(group => group
                .OrderByDescending(trl => trl.LastEventAt)
                .First())
            .ToList();
    }

    public async Task<IEnumerable<TrailerResultDto>> GetByCityAndStateAsync(string city = null, string state = null)
    {
        var trailers = _unitOfWork.TrailerRepository.SelectAll();

        if (!string.IsNullOrEmpty(city))
            trailers = trailers.Where(t => t.Address.City.ToLower().Contains(city.ToLower()));
        if (!string.IsNullOrEmpty(state))
            trailers = trailers.Where(t => t.Address.State.ToLower().Contains(state.ToLower()));

        return _mapper.Map<List<TrailerResultDto>>(trailers);
    }
}
