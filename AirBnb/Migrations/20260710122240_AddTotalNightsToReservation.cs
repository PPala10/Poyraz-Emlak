using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnb.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalNightsToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_nights",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_nights",
                table: "Reservations");
        }
    }
}
