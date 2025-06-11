using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtelUçakRezervasyon.Migrations
{
    /// <inheritdoc />
    public partial class AddKimlikAndDogumTarihiToHotelReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "HotelReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TcKimlikNo",
                table: "HotelReservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "FlightsReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TcKimlikNo",
                table: "FlightsReservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "HotelReservations");

            migrationBuilder.DropColumn(
                name: "TcKimlikNo",
                table: "HotelReservations");

            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "FlightsReservations");

            migrationBuilder.DropColumn(
                name: "TcKimlikNo",
                table: "FlightsReservations");
        }
    }
}
