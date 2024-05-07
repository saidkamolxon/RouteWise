using AutoMapper;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class TruckService : ITruckService
{
    private readonly IGoogleMapsService _googleMapsService;
    private readonly ISwiftEldService _swiftEldService;
    private readonly IMapper _mapper;

    public TruckService(IGoogleMapsService googleMapsService,
        ISwiftEldService swiftEldService, IMapper mapper)
    {
        _googleMapsService = googleMapsService;
        _swiftEldService = swiftEldService;
        _mapper = mapper;
    }

    public async Task<TruckResultDto> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        var truck = await _swiftEldService.GetTruckStateByNameAsync(name, cancellationToken);
        truck.Address = await _googleMapsService.GetReverseGeocodingAsync(truck.Coordinates.ToString(), cancellationToken);
        truck.LastEventAt = truck.LastEventAt.ConvertUtcToDefaultTime();

        return _mapper.Map<TruckResultDto>(truck);
    }

    public async Task<IEnumerable<string>> GetTruckNumbersAsync(CancellationToken cancellationToken = default)
    {
        return await _swiftEldService.GetAllTruckNumbersAsync(cancellationToken);
    }
}
