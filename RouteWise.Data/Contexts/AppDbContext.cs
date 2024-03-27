﻿using Microsoft.EntityFrameworkCore;
using RouteWise.Domain.Entities;

namespace RouteWise.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var trailers = modelBuilder.Entity<Trailer>();
        var landmarks = modelBuilder.Entity<Landmark>();

        trailers.OwnsOne(t => t.Address);
        trailers.HasIndex(t => t.Name).IsUnique();
        trailers.HasIndex(t => t.Vin).IsUnique();
        trailers.HasIndex(t => t.License).IsUnique();

        landmarks.OwnsOne(l => l.Address);
        landmarks.HasIndex(l => l.Name).IsUnique();
    }

    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Landmark> Landmarks { get; set; }
    public DbSet<Trailer> Trailers { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<User> Users { get; set; }
}
