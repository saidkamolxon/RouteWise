using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RouteWise.Data.IRepositories;
using RouteWise.Service.Interfaces;
using System.Globalization;
using AutoMapper;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Landmark;

namespace RouteWise.Service.Services;

public class LandmarkService : ILandmarkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFleetLocateService _service;
    private readonly IMapper _mapper;

    public LandmarkService(IUnitOfWork unitOfWork, IFleetLocateService service, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _service = service;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LandmarkResultDto>> GetLandmarksByNameAsync(string name)
    {
        var landmarks = await _unitOfWork.LandmarkRepository.SelectAll(l =>
            l.Name.ToUpper()
                .Contains(name.ToUpper()))
            .ToListAsync();
        return _mapper.Map<IEnumerable<LandmarkResultDto>>(landmarks);
    }

    public async Task<IEnumerable<LandmarkResultDto>> GetAllLandmarksAsync()
    {
        var landmarks = await _unitOfWork.LandmarkRepository.SelectAll(asNoTracking:true).ToListAsync();
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

    public async Task<int> GetLandmarkIdOrDefaultAsync(string state, Domain.Models.Coordinate coordinates)
    {
        var landmarks = _unitOfWork.LandmarkRepository
            .SelectAll(landmark => landmark.Address.State
            .Equals(state, StringComparison.OrdinalIgnoreCase));

        var landmark = await landmarks.FirstOrDefaultAsync(landmark =>
            IsAssetWithinLandmark(landmark.BorderPoints, coordinates));

        if (landmark is not null) 
            return landmark.Id;

        return default;
    }

    private static bool IsAssetWithinLandmark(IEnumerable<Domain.Models.Coordinate> landmarkBorders, Domain.Models.Coordinate assetCoordinates)
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
        => factory.CreatePolygon(Array.ConvertAll(borders.ToArray(), b => new Coordinate { X = b.Latitude, Y = b.Longitude }));

    private static Point CreateAssetPoint(Domain.Models.Coordinate coordinates, GeometryFactory factory)
        => factory.CreatePoint(new Coordinate { X = coordinates.Latitude, Y = coordinates.Longitude });
}
