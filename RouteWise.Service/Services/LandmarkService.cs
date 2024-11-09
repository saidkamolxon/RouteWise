using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;
using RouteWise.Service.Brokers.APIs.FleetLocate;
using RouteWise.Service.Brokers.APIs.GoogleMaps;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class LandmarkService(
    ILogger<LandmarkService> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IFleetLocateApiBroker fleetLocateApiBroker,
    IGoogleMapsApiBroker googleMapsApiBroker) : ILandmarkService
{
    private readonly ILogger<LandmarkService> logger = logger;
    private readonly IMapper mapper = mapper;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IFleetLocateApiBroker fleetLocateApiBroker = fleetLocateApiBroker;
    private readonly IGoogleMapsApiBroker googleMapsApiBroker = googleMapsApiBroker;

    public async Task<IEnumerable<LandmarkResultDto>> GetLandmarksByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var landmarks = await unitOfWork
            .LandmarkRepository
            .SelectAll(l => l.Name.ToUpper().Contains(name.ToUpper()), includes: ["Trailers"])
            .OrderBy(l => l.Name)
            .ToListAsync();

        var mappedLandmarks = mapper.Map<IEnumerable<LandmarkResultDto>>(landmarks);
        
        foreach (var landmark in mappedLandmarks)
            landmark.PhotoUrl = await googleMapsApiBroker
                .GetStaticMapAsync(landmark.Coordinates,
                landmark.Trailers
                    .OrderBy(t => t.ArrivedAt).Select(t => t.Coordinates)
                    .ToArray(),
                cancellationToken);

        return mappedLandmarks;
    }

    public async Task<IEnumerable<LandmarkResultDto>> GetAllLandmarksAsync(CancellationToken cancellationToken = default)
    {
        var landmarks = await unitOfWork.LandmarkRepository.SelectAll(includes: ["Trailers"]).ToListAsync();
        return mapper.Map<IEnumerable<LandmarkResultDto>>(landmarks);
    }

    public async Task UpdateLandmarksAsync(CancellationToken cancellationToken = default)
    {
        var landmarkUpdates = await fleetLocateApiBroker.GetLandmarksAsync(cancellationToken);
        foreach (var update in landmarkUpdates)
        {
            var landmark = await unitOfWork.LandmarkRepository.SelectAsync(l => l.Name.Equals(update.Name));
            if (landmark is not null)
            {
                mapper.Map(update, landmark);
                unitOfWork.LandmarkRepository.Update(landmark);
            }
            else
            {
                var newLandmark = mapper.Map<Landmark>(update);
                await unitOfWork.LandmarkRepository.CreateAsync(newLandmark);
            }
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }

    public async Task<int?> GetLandmarkIdOrDefaultAsync(string state, Coordination coordinates, CancellationToken cancellationToken = default)
    {
        var landmarks = await unitOfWork.LandmarkRepository
            .SelectAll(landmark => landmark.Address.State
                .Equals(state), asNoTracking:true)
            .ToListAsync(cancellationToken);

        var landmark = landmarks.FirstOrDefault(landmark =>
            IsAssetWithinLandmark(landmark.BorderPoints, coordinates));

        return landmark?.Id;
    }

    private bool IsAssetWithinLandmark(IEnumerable<Coordination> landmarkBorders, Coordination assetCoordinates)
    {
        try
        {
            var factory = new GeometryFactory();
            var landmark = createLandmarkPolygon(landmarkBorders, factory);
            var asset = createAssetPoint(assetCoordinates, factory);
            return landmark.Contains(asset);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while checking if an asset is within a landmark.");
            return false;
        }
    }

    private Polygon createLandmarkPolygon(IEnumerable<Coordination> borders, GeometryFactory factory)
    {
        var bordersArray = borders.ToArray();

        if (bordersArray.First() != bordersArray.Last())
            bordersArray = [.. bordersArray, bordersArray.First()];

        return factory.CreatePolygon(Array.ConvertAll(bordersArray, b =>
            new Coordinate { X = b.Latitude, Y = b.Longitude }));
    }

    private Point createAssetPoint(Coordination coordinates, GeometryFactory factory)
        => factory.CreatePoint(new Coordinate { X = coordinates.Latitude, Y = coordinates.Longitude });

    public async Task RemoveRedundantLandmarks(CancellationToken cancellationToken = default)
    {
        var theDayBefore = DateTime.UtcNow - TimeSpan.FromHours(24);
        var rowsDeleted = await this.unitOfWork.LandmarkRepository.DestroyAllAsync(l => l.UpdatedAt < theDayBefore, cancellationToken);
        if (rowsDeleted != null)
            this.logger.LogInformation("{count} redundant landmarks deleted from the database", rowsDeleted);
    }
}
