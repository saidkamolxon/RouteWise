using RouteWise.Domain.Entities;
using RouteWise.Domain.Models;
using RouteWise.Service.Interfaces;
using RouteWise.Service.Services;

namespace UnitTests;

[TestClass]
public class LandmarkServiceTests
{
    [TestMethod]
    public void IsAssetWithinLandmark_ShouldReturnTrue()
    {
        var trailer = new Trailer
        {
            Name = "blah",
            Coordinates = new Coordinate()
            {
                Latitude = 5,
                Longitude = 5
            }
        };

        var landmark = new Landmark
        {
            Name = "AVP9",
            BorderPoints = new List<Coordinate>()
            {
                new Coordinate() { Latitude = 0, Longitude = 0 },
                new Coordinate() { Latitude = 0, Longitude = 10 },
                new Coordinate() { Latitude = 10, Longitude = 10 },
                new Coordinate() { Latitude = 10, Longitude = 0 },
                new Coordinate() { Latitude = 0, Longitude = 0 }
            }
        };

        var result = LandmarkService.IsAssetWithinLandmark(landmark.BorderPoints, trailer.Coordinates);

        Assert.AreEqual(true, result);
    }
}