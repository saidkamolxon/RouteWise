﻿using Microsoft.EntityFrameworkCore;
using RouteWise.Domain.Entities;
using RouteWise.Domain.Enums;

namespace RouteWise.Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Landmark> Landmarks { get; set; }
    public DbSet<Trailer> Trailers { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<State> States { get; set; }

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
        trailers
            .HasOne(t => t.Landmark)
            .WithMany(l => l.Trailers)
            .HasForeignKey(t => t.LandmarkId)
            .OnDelete(DeleteBehavior.SetNull);

        
        var trucks = modelBuilder.Entity<Truck>();
        trucks.OwnsOne(t => t.Address);
        trucks.OwnsOne(t => t.Coordinates);
        trucks
            .HasOne(t => t.Landmark)
            .WithMany(l => l.Trucks)
            .HasForeignKey(t => t.LandmarkId)
            .OnDelete(DeleteBehavior.SetNull);


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


        var users = modelBuilder.Entity<User>();
        users.OwnsOne(u => u.StepValue);

        #region SeedData
        users.HasData(new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Saidkamol",
                LastName = "Saidjamolov",
                TelegramId = 5885255512,
                Role = UserRole.SuperAdmin
            }
        });
        #endregion
    }
}
