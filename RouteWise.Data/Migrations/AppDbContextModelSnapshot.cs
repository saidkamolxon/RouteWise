﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RouteWise.Data.Contexts;

#nullable disable

namespace RouteWise.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("RouteWise.Domain.Entities.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DitatId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("SamsaraId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SwiftEldId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Landmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BorderPointsJson")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Landmarks");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DistanceDestination")
                        .HasColumnType("TEXT");

                    b.Property<string>("DistanceOrigin")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerializedState")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Trailer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ArrivedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMoving")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LandmarkId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastEventAt")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("LastInspectionOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("License")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("NextInspectionOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Vin")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LandmarkId");

                    b.HasIndex("License")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Vin")
                        .IsUnique();

                    b.ToTable("Trailers");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Truck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LandmarkId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastEventAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("License")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Odometer")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ownership")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Vin")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LandmarkId");

                    b.ToTable("Trucks");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentStep")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TelegramId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 11, 9, 11, 13, 23, 120, DateTimeKind.Utc).AddTicks(1953),
                            CurrentStep = 0,
                            FirstName = "Saidkamol",
                            IsDeleted = false,
                            LastName = "Saidjamolov",
                            Role = 2,
                            TelegramId = 5885255512L
                        });
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Landmark", b =>
                {
                    b.OwnsOne("RouteWise.Domain.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("LandmarkId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .HasColumnType("TEXT");

                            b1.Property<string>("State")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("TEXT");

                            b1.HasKey("LandmarkId");

                            b1.ToTable("Landmarks");

                            b1.WithOwner()
                                .HasForeignKey("LandmarkId");
                        });

                    b.OwnsOne("RouteWise.Domain.Models.Coordination", "Coordinates", b1 =>
                        {
                            b1.Property<int>("LandmarkId")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Latitude")
                                .HasColumnType("REAL");

                            b1.Property<double>("Longitude")
                                .HasColumnType("REAL");

                            b1.HasKey("LandmarkId");

                            b1.ToTable("Landmarks");

                            b1.WithOwner()
                                .HasForeignKey("LandmarkId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Coordinates")
                        .IsRequired();
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Trailer", b =>
                {
                    b.HasOne("RouteWise.Domain.Entities.Landmark", "Landmark")
                        .WithMany("Trailers")
                        .HasForeignKey("LandmarkId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("RouteWise.Domain.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("TrailerId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .HasColumnType("TEXT");

                            b1.Property<string>("State")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("TEXT");

                            b1.HasKey("TrailerId");

                            b1.ToTable("Trailers");

                            b1.WithOwner()
                                .HasForeignKey("TrailerId");
                        });

                    b.OwnsOne("RouteWise.Domain.Models.Coordination", "Coordinates", b1 =>
                        {
                            b1.Property<int>("TrailerId")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Latitude")
                                .HasColumnType("REAL");

                            b1.Property<double>("Longitude")
                                .HasColumnType("REAL");

                            b1.HasKey("TrailerId");

                            b1.ToTable("Trailers");

                            b1.WithOwner()
                                .HasForeignKey("TrailerId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Coordinates")
                        .IsRequired();

                    b.Navigation("Landmark");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Truck", b =>
                {
                    b.HasOne("RouteWise.Domain.Entities.Landmark", "Landmark")
                        .WithMany("Trucks")
                        .HasForeignKey("LandmarkId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("RouteWise.Domain.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("TruckId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .HasColumnType("TEXT");

                            b1.Property<string>("State")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("TEXT");

                            b1.HasKey("TruckId");

                            b1.ToTable("Trucks");

                            b1.WithOwner()
                                .HasForeignKey("TruckId");
                        });

                    b.OwnsOne("RouteWise.Domain.Models.Coordination", "Coordinates", b1 =>
                        {
                            b1.Property<int>("TruckId")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Latitude")
                                .HasColumnType("REAL");

                            b1.Property<double>("Longitude")
                                .HasColumnType("REAL");

                            b1.HasKey("TruckId");

                            b1.ToTable("Trucks");

                            b1.WithOwner()
                                .HasForeignKey("TruckId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Coordinates");

                    b.Navigation("Landmark");
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.User", b =>
                {
                    b.OwnsOne("RouteWise.Domain.Models.StepValue", "StepValue", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Destination")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Origin")
                                .HasColumnType("TEXT");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("StepValue")
                        .IsRequired();
                });

            modelBuilder.Entity("RouteWise.Domain.Entities.Landmark", b =>
                {
                    b.Navigation("Trailers");

                    b.Navigation("Trucks");
                });
#pragma warning restore 612, 618
        }
    }
}
