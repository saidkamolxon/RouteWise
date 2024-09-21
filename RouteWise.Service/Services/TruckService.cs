﻿using AutoMapper;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class TruckService(IGoogleMapsService googleMapsService,
    ISwiftEldService swiftEldService, IMapper mapper) : ITruckService
{
    private readonly IGoogleMapsService _googleMapsService = googleMapsService;
    private readonly ISwiftEldService _swiftEldService = swiftEldService;
    private readonly IMapper _mapper = mapper;

    public async Task<TruckResultDto> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var truck = await _swiftEldService.GetTruckStateByNameAsync(name, cancellationToken);
        truck.Address = await _googleMapsService.GetReverseGeocodingAsync(truck.Coordinates.ToString(), cancellationToken);
        truck.LastEventAt = truck.LastEventAt.ConvertUtcToDefaultTime();
        
        var result = _mapper.Map<TruckResultDto>(truck);
        result.PhotoUrl = await _googleMapsService.GetStaticMapAsync(truck.Coordinates.ToString(), "https://i.ibb.co/nBKXZK7/semi-truck-pin-icon.png", cancellationToken);
        return result;
    }

    public async Task<IEnumerable<string>> GetTruckNumbersAsync(CancellationToken cancellationToken = default)
    {
        return await _swiftEldService.GetAllTruckNumbersAsync(cancellationToken);
    }
}
