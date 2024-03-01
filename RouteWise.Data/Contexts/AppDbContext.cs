using Microsoft.EntityFrameworkCore;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    DbSet<Driver> Drivers { get; set; }
    DbSet<Landmark> Landmarks { get; set; }
    DbSet<Trailer> Trailers { get; set; }
    DbSet<Truck> Trucks { get; set; }
    DbSet<User> Users { get; set; }
}
