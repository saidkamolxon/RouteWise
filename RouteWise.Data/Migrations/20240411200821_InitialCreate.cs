using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RouteWise.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Landmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Coordinates_Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Coordinates_Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Address_Street = table.Column<string>(type: "TEXT", nullable: true),
                    Address_City = table.Column<string>(type: "TEXT", nullable: true),
                    Address_State = table.Column<string>(type: "TEXT", nullable: true),
                    Address_ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    BorderPointsJson = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentStep = table.Column<int>(type: "INTEGER", nullable: false),
                    StepValue_Origin = table.Column<string>(type: "TEXT", nullable: true),
                    StepValue_Destination = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trailers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Coordinates_Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Coordinates_Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Address_Street = table.Column<string>(type: "TEXT", nullable: true),
                    Address_City = table.Column<string>(type: "TEXT", nullable: true),
                    Address_State = table.Column<string>(type: "TEXT", nullable: true),
                    Address_ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    Vin = table.Column<string>(type: "TEXT", nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: true),
                    License = table.Column<string>(type: "TEXT", nullable: true),
                    IsMoving = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastEventAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastInspectionOn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    NextInspectionOn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    LandmarkId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trailers_Landmarks_LandmarkId",
                        column: x => x.LandmarkId,
                        principalTable: "Landmarks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    License = table.Column<string>(type: "TEXT", nullable: true),
                    Vin = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: true),
                    Address_Street = table.Column<string>(type: "TEXT", nullable: true),
                    Address_City = table.Column<string>(type: "TEXT", nullable: true),
                    Address_State = table.Column<string>(type: "TEXT", nullable: true),
                    Address_ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    LastEventAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Coordinates_Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Coordinates_Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Odometer = table.Column<long>(type: "INTEGER", nullable: true),
                    LandmarkId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trucks_Landmarks_LandmarkId",
                        column: x => x.LandmarkId,
                        principalTable: "Landmarks",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CurrentStep", "FirstName", "IsDeleted", "LastName", "Role", "TelegramId", "UpdatedAt", "UserName" },
                values: new object[] { 1, new DateTime(2024, 4, 11, 20, 8, 21, 858, DateTimeKind.Utc).AddTicks(8796), 0, "Saidkamol", false, "Saidjamolov", 2, 5885255512L, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Landmarks_Name",
                table: "Landmarks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_LandmarkId",
                table: "Trailers",
                column: "LandmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_License",
                table: "Trailers",
                column: "License",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_Name",
                table: "Trailers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_Vin",
                table: "Trailers",
                column: "Vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_LandmarkId",
                table: "Trucks",
                column: "LandmarkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Trailers");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Landmarks");
        }
    }
}
