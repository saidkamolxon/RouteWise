using Microsoft.EntityFrameworkCore;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var trailers = modelBuilder.Entity<Trailer>();
        trailers.OwnsOne(t => t.Address);
        trailers.OwnsOne(t => t.Coordinates);
        trailers
            .HasIndex(t => t.Name)
            .IsUnique();
        trailers
            .HasIndex(t => t.Vin)
            .IsUnique();
        trailers
            .HasIndex(t => t.License)
            .IsUnique();

        
        var trucks = modelBuilder.Entity<Truck>();
        trucks.OwnsOne(t => t.Address);
        trucks.OwnsOne(t => t.Coordinates);



        var landmarks = modelBuilder.Entity<Landmark>();
        landmarks.OwnsOne(l => l.Address);
        landmarks.OwnsOne(l => l.Coordinates);
        landmarks
            .HasMany(l => l.Trailers)
            .WithOne(t => t.Landmark)
            .HasForeignKey(t => t.LandmarkId);
        landmarks
            .HasIndex(l => l.Name)
            .IsUnique();
    }

    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Landmark> Landmarks { get; set; }
    public DbSet<Trailer> Trailers { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<User> Users { get; set; }
}
