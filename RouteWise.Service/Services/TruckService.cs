using AutoMapper;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.Brokers.APIs.GoogleMaps;
using RouteWise.Service.Brokers.APIs.Samsara;
using RouteWise.Service.Brokers.APIs.SwiftEld;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.Helpers;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class TruckService(IMapper mapper, IUnitOfWork unitOfWork, IGoogleMapsApiBroker googleMapsService,
    ISwiftEldApiBroker swiftEldService, ISamsaraApiBroker samsaraService, ILandmarkService landmarkService) : ITruckService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGoogleMapsApiBroker _googleMapsService = googleMapsService;
    private readonly ISwiftEldApiBroker _swiftEldService = swiftEldService;
    private readonly ISamsaraApiBroker _samsaraService = samsaraService;
    private readonly ILandmarkService _landmarkService = landmarkService;

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

    public async Task UpdateTrucksStatesAsync(CancellationToken cancellationToken = default)
    {
        var trucksStates = await this.getTruckStatesAsync(cancellationToken);

        foreach (var state in trucksStates)
        {
            var truck = await _unitOfWork.TruckRepository.SelectAsync(t => t.Name.Equals(state.Name));
            if (truck is not null)
            {
                _mapper.Map(state, truck);
                truck.LandmarkId = await _landmarkService.GetLandmarkIdOrDefaultAsync(truck.Address.State, truck.Coordinates);
                _unitOfWork.TruckRepository.Update(truck);
            }
            else
            {
                var newTruck = _mapper.Map<Truck>(state);
                newTruck.LandmarkId = await _landmarkService.GetLandmarkIdOrDefaultAsync(newTruck.Address.State, newTruck.Coordinates);
                await _unitOfWork.TruckRepository.CreateAsync(newTruck);
            }
            await _unitOfWork.SaveAsync();
        }
    }

    private async Task<List<TruckStateDto>> getTruckStatesAsync(CancellationToken cancellationToken = default)
    {
        var swiftTrucks = await _swiftEldService.GetAllTrucksStatesAsync(cancellationToken);
        var samsaraTrucks = await _samsaraService.GetAllTrucksStatesAsync(cancellationToken);

        return swiftTrucks
            .Concat(samsaraTrucks)
            .GroupBy(truck => truck.Name)
            .Select(group => group
                .OrderByDescending(truck => truck.LastEventAt)
                .First())
            .ToList();
    }
}
