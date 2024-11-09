using Microsoft.EntityFrameworkCore;
using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;

namespace RouteWise.Data.Repositories;

public class LandmarkRepository(AppDbContext appDbContext) : Repository<Landmark>(appDbContext), ILandmarkRepository
{
    private readonly AppDbContext appDbContext = appDbContext;

    //public IQueryable<Landmark> GetClosestLandmarks(Coordinate coordinates) =>
    //    appDbContext.Landmarks.Where(l =>
    //        (int)l.Coordinates.Latitude == (int)coordinates.Latitude &&
    //            (int)l.Coordinates.Longitude == (int)coordinates.Longitude);

    public IQueryable<Landmark> GetClosestLandmarks(Coordination coordinates, double rangeInDegrees = 0.01)
    {
        double latMin = coordinates.Latitude - rangeInDegrees;
        double latMax = coordinates.Latitude + rangeInDegrees;
        double lonMin = coordinates.Longitude - rangeInDegrees;
        double lonMax = coordinates.Longitude + rangeInDegrees;

        return appDbContext.Landmarks.Where(l =>
            l.Coordinates.Latitude >= latMin &&
            l.Coordinates.Latitude <= latMax &&
            l.Coordinates.Longitude >= lonMin &&
            l.Coordinates.Longitude <= lonMax);
    }
}
