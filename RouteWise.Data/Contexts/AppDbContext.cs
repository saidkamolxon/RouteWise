using Microsoft.EntityFrameworkCore;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Contexts;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    DbSet<Driver> Drivers { get; set; }
    DbSet<Landmark> Landmarks { get; set; }
    DbSet<Trailer> Trailers { get; set; }
    DbSet<Truck> Trucks { get; set; }
    DbSet<User> Users { get; set; }
}
