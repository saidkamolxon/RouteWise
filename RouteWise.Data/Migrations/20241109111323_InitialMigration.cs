using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RouteWise.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Landmarks_LandmarkId",
                table: "Trailers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Landmarks_LandmarkId",
                table: "Trucks");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "Drivers",
                newName: "SwiftEldId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Trucks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<double>(
                name: "Coordinates_Longitude",
                table: "Trucks",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "Coordinates_Latitude",
                table: "Trucks",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "Ownership",
                table: "Trucks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivedAt",
                table: "Trailers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DitatId",
                table: "Drivers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SamsaraId",
                table: "Drivers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 9, 11, 13, 23, 120, DateTimeKind.Utc).AddTicks(1953));

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_Landmarks_LandmarkId",
                table: "Trailers",
                column: "LandmarkId",
                principalTable: "Landmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Landmarks_LandmarkId",
                table: "Trucks",
                column: "LandmarkId",
                principalTable: "Landmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Landmarks_LandmarkId",
                table: "Trailers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Landmarks_LandmarkId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "Ownership",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "ArrivedAt",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "DitatId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "SamsaraId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "SwiftEldId",
                table: "Drivers",
                newName: "DriverId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Trucks",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Coordinates_Longitude",
                table: "Trucks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Coordinates_Latitude",
                table: "Trucks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 4, 13, 20, 35, 4, 782, DateTimeKind.Utc).AddTicks(5318));

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_Landmarks_LandmarkId",
                table: "Trailers",
                column: "LandmarkId",
                principalTable: "Landmarks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Landmarks_LandmarkId",
                table: "Trucks",
                column: "LandmarkId",
                principalTable: "Landmarks",
                principalColumn: "Id");
        }
    }
}
