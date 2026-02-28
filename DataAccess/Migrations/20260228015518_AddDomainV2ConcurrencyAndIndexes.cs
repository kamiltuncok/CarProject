using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainV2ConcurrencyAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rentals_Car_Dates",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CurrentLocationId",
                table: "Cars");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Cars",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_AvailabilityCheck",
                table: "Rentals",
                columns: new[] { "CarId", "StartDate", "EndDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_LocationStatus",
                table: "Cars",
                columns: new[] { "CurrentLocationId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rentals_AvailabilityCheck",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Cars_LocationStatus",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Cars");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_Car_Dates",
                table: "Rentals",
                columns: new[] { "CarId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CurrentLocationId",
                table: "Cars",
                column: "CurrentLocationId");
        }
    }
}
