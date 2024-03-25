﻿using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Trailer;
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

    public async Task<int> GetLandmarkIdOrDefaultAsync(TrailerStateDto asset)
    {
        var landmarks = _unitOfWork.LandmarkRepository
            .SelectAll(landmark => landmark.Address.State
            .Equals(asset.Address.State, StringComparison.OrdinalIgnoreCase));

        var landmark = await landmarks.FirstOrDefaultAsync(landmark =>
            IsAssetWithinLandmark(landmark.BorderPoints, asset.Coordinates));

        if (landmark is not null) 
            return landmark.Id;
        return default;
    }

    private static bool IsAssetWithinLandmark(string landmarkBorders, string assetCoordinates)
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

    private static Polygon CreateLandmarkPolygon(string landmarkBorders, GeometryFactory factory)
    {
        var coordinates = Array.ConvertAll(landmarkBorders.Split(','), b =>
        {
            string[] xy = b.Trim().Split();
            return new Coordinate(double.Parse(xy[1], CultureInfo.InvariantCulture),
                                  double.Parse(xy[0], CultureInfo.InvariantCulture));
        });
        if (coordinates.Length < 3)
            throw new ArgumentException("Invalid landmark borders");
        return factory.CreatePolygon(coordinates.Append(coordinates[0]).ToArray());
    }

    private static Point CreateAssetPoint(string assetCoordinates, GeometryFactory factory)
    {
        var pointCoordinates = Array.ConvertAll(assetCoordinates.Split(','), c =>
            double.Parse(c.Trim(), CultureInfo.InvariantCulture));
        return factory.CreatePoint(new Coordinate(pointCoordinates[1], pointCoordinates[0]));
    }
}
