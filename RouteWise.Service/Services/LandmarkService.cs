using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class LandmarkService : ILandmarkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFleetLocateService _service;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IMapper _mapper;

    public LandmarkService(IUnitOfWork unitOfWork, IMapper mapper,
                           IFleetLocateService service,
                           IGoogleMapsService googleMapsService)
    {
        _unitOfWork = unitOfWork;
        _service = service;
        _googleMapsService = googleMapsService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LandmarkResultDto>> GetLandmarksByNameAsync(string name)
    {
        var landmarks = await _unitOfWork
            .LandmarkRepository
            .SelectAll(l => l.Name.ToUpper().Contains(name.ToUpper()), includes: ["Trailers"])
            .OrderBy(l => l.Name)
            .ToListAsync();

        var mappedLandmarks = _mapper.Map<IEnumerable<LandmarkResultDto>>(landmarks);
        
        foreach (var landmark in mappedLandmarks)
            landmark.PhotoUrl = await _googleMapsService.GetStaticMapAsync(landmark.Coordinates, landmark.Trailers.Select(t => t.Coordinates).ToArray());

        return mappedLandmarks;
    }

    public async Task<IEnumerable<LandmarkResultDto>> GetAllLandmarksAsync()
    {
        var landmarks = await _unitOfWork.LandmarkRepository.SelectAll(includes: ["Trailers"]).ToListAsync();
        return _mapper.Map<IEnumerable<LandmarkResultDto>>(landmarks);
    }

    public async Task UpdateLandmarksAsync()
    {
        var landmarkUpdates = await _service.GetLandmarksAsync();
        foreach (var update in landmarkUpdates)
        {
            var landmark = await _unitOfWork.LandmarkRepository.SelectAsync(l => l.Name.Equals(update.Name));
            if (landmark is not null)
            {
                _mapper.Map(update, landmark);
                _unitOfWork.LandmarkRepository.Update(landmark);
            }
            else
            {
                var newLandmark = _mapper.Map<Landmark>(update);
                await _unitOfWork.LandmarkRepository.CreateAsync(newLandmark);
            }
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task<int?> GetLandmarkIdOrDefaultAsync(string state, Domain.Models.Coordinate coordinates)
    {
        var landmarks = await _unitOfWork.LandmarkRepository
            .SelectAll(landmark => landmark.Address.State
                .Equals(state), asNoTracking:true)
            .ToListAsync();

        var landmark = landmarks.FirstOrDefault(landmark =>
            IsAssetWithinLandmark(landmark.BorderPoints, coordinates));

        return landmark?.Id;
    }

    public static bool IsAssetWithinLandmark(IEnumerable<Domain.Models.Coordinate> landmarkBorders, Domain.Models.Coordinate assetCoordinates) //TODO need to make private
    {
        try
        {
            var factory = new GeometryFactory();
            var landmark = CreateLandmarkPolygon(landmarkBorders, factory);
            var asset = CreateAssetPoint(assetCoordinates, factory);
            return landmark.Contains(asset);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private static Polygon CreateLandmarkPolygon(IEnumerable<Domain.Models.Coordinate> borders, GeometryFactory factory)
    {
        var bordersArray = borders.ToArray();

        if (bordersArray.First() != bordersArray.Last())
            bordersArray = bordersArray.Append(bordersArray.First()).ToArray();

        return factory.CreatePolygon(Array.ConvertAll(bordersArray, b =>
            new Coordinate { X = b.Latitude, Y = b.Longitude }));
    }

    private static Point CreateAssetPoint(Domain.Models.Coordinate coordinates, GeometryFactory factory)
        => factory.CreatePoint(new Coordinate { X = coordinates.Latitude, Y = coordinates.Longitude });
}
