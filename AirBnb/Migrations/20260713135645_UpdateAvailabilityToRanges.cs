using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvailabilityToRanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Availabilities",
                newName: "start_date");

            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "Availabilities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_date",
                table: "Availabilities");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Availabilities",
                newName: "date");
        }
    }
}
