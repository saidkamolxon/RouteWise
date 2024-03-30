using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RouteWise.Data.IRepositories;
using RouteWise.Service.Interfaces;
using System.Globalization;

namespace RouteWise.Service.Services;

public class LandmarkService : ILandmarkService
{
    private readonly IUnitOfWork _unitOfWork;

    public LandmarkService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
