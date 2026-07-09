using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnb.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amenities",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "price_per_knight",
                table: "Listings",
                newName: "price_per_night");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price_per_night",
                table: "Listings",
                newName: "price_per_knight");

            migrationBuilder.AddColumn<string>(
                name: "amenities",
                table: "Listings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                table: "Listings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                table: "Listings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
