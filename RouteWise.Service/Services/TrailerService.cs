using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.Brokers.APIs.FleetLocate;
using RouteWise.Service.Brokers.APIs.GoogleMaps;
using RouteWise.Service.Brokers.APIs.RoadReady;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.Exceptions;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class TrailerService : ITrailerService
{
    private readonly ILogger<TrailerService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFleetLocateApiBroker _fleetLocateService;
    private readonly IRoadReadyApiBroker _roadReadyService;
    private readonly ILandmarkService _landmarkService;
    private readonly IGoogleMapsApiBroker _googleMapsService;

    public TrailerService(ILogger<TrailerService> logger,
                          IUnitOfWork unitOfWork, IMapper mapper,
                          IFleetLocateApiBroker fleetLocateService,
                          IRoadReadyApiBroker roadReadyService,
                          ILandmarkService landmarkService,
                          IGoogleMapsApiBroker googleMapsService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fleetLocateService = fleetLocateService;
        _roadReadyService = roadReadyService;
        _landmarkService = landmarkService;
        _googleMapsService = googleMapsService;
    }

    public async Task<TrailerResultDto> CreateAsync(TrailerCreationDto dto, CancellationToken cancellationToken = default)
    {
        var newTrailer = _mapper.Map<Trailer>(dto);
        await _unitOfWork.TrailerRepository.CreateAsync(newTrailer);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<TrailerResultDto>(newTrailer);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(id)
             ?? throw new NotFoundException("Trailer with such id is not found.");
        
        _unitOfWork.TrailerRepository.Delete(trailer);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IReadOnlyList<TrailerResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var trailers = await _unitOfWork.TrailerRepository
                                .SelectAll(asNoTracking: true, includes: ["Landmark"])
                                .OrderBy(t => t.Name)
                                .ToListAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TrailerResultDto>>(trailers);
    }

    public async Task<TrailerResultDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    { 
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(id, includes: ["Landmark"])
            ?? throw new NotFoundException("Trailer with such id is not found.");

        return _mapper.Map<TrailerResultDto>(trailer);
    }

    public async Task<TrailerResultDto> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var trailer = await _unitOfWork.TrailerRepository
            .SelectAsync(t => t.Name.ToLower().Contains(name.ToLower()), includes: ["Landmark"])
                ?? throw new NotFoundException("Trailer with such name is not found.");

        var dto = _mapper.Map<TrailerResultDto>(trailer);
        dto.PhotoUrl = await _googleMapsService.GetStaticMapAsync(dto.Coordinates, cancellationToken: cancellationToken);

        if (trailer.LandmarkId != null && trailer.LandmarkId != 0)
            dto.Address = trailer.Landmark.Address.ToString();

        return dto;
    }

    public async Task<TrailerResultDto> UpdateAsync(TrailerStateDto dto, CancellationToken cancellationToken = default)
    {
        var trailer = await _unitOfWork.TrailerRepository.SelectAsync(1);
        var mappedTrailer = _mapper.Map(dto, trailer);

        _unitOfWork.TrailerRepository.Update(mappedTrailer);
        await _unitOfWork.SaveAsync();

        var updatedTrailer = await _unitOfWork.TrailerRepository.SelectAsync(mappedTrailer.Id);
        return _mapper.Map<TrailerResultDto>(updatedTrailer);
    }

    public async Task UpdateTrailersStatesAsync(CancellationToken cancellationToken = default)
    {
        var trailerStates = MergeData(await _fleetLocateService.GetTrailersStatesAsync(), 
                                      await _roadReadyService.GetTrailersStatesAsync());

        foreach (var state in trailerStates)
        {
            var trailer = await _unitOfWork.TrailerRepository.SelectAsync(t => t.Name.Equals(state.Name));
            if (trailer is not null)
            {
                _mapper.Map(state, trailer);
                
                var landmarkId = await _landmarkService.GetLandmarkIdOrDefaultAsync(trailer.Address.State, trailer.Coordinates);

                if (landmarkId != trailer.LandmarkId)
                {
                    trailer.LandmarkId = landmarkId;
                    trailer.ArrivedAt = landmarkId == null ? null : trailer.LastEventAt;
                }

                if (landmarkId != null && trailer.ArrivedAt == null)
                    trailer.ArrivedAt = trailer.LastEventAt;

                _unitOfWork.TrailerRepository.Update(trailer);
            }
            else
            {
                var newTrailer = _mapper.Map<Trailer>(state);
                newTrailer.LandmarkId = await _landmarkService.GetLandmarkIdOrDefaultAsync(newTrailer.Address.State, newTrailer.Coordinates);
                newTrailer.ArrivedAt = state.LastEventAt;
                await _unitOfWork.TrailerRepository.CreateAsync(newTrailer);
            }
            await _unitOfWork.SaveAsync();
        }

        _logger.LogInformation("Trailers states have been updated.");
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

    public async Task<IEnumerable<TrailerResultDto>> GetByCityAndStateAsync(string city = null, string state = null, CancellationToken cancellationToken = default)
    {
        var trailers = _unitOfWork.TrailerRepository.SelectAll();

        if (!string.IsNullOrEmpty(city))
            trailers = trailers.Where(t => t.Address.City.Contains(city, StringComparison.CurrentCultureIgnoreCase));
        if (!string.IsNullOrEmpty(state))
            trailers = trailers.Where(t => t.Address.State.Contains(state, StringComparison.CurrentCultureIgnoreCase));

        return await Task.FromResult(_mapper.Map<List<TrailerResultDto>>(trailers));
    }
}
