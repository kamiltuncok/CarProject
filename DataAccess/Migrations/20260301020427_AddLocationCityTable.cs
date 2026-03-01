using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationCityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Step 1: Create the new lookup table ──────────────────────────────
            migrationBuilder.CreateTable(
                name: "LocationCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationCities_Name",
                table: "LocationCities",
                column: "Name",
                unique: true);

            // ── Step 2: Seed the three cities (Id = 1, 2, 3) ────────────────────
            migrationBuilder.InsertData(
                table: "LocationCities",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "İstanbul" },
                    { "Ankara" },
                    { "İzmir" }
                });

            // ── Step 3: Add the FK column as nullable so existing rows don't fail─
            migrationBuilder.AddColumn<int>(
                name: "LocationCityId",
                table: "Locations",
                type: "int",
                nullable: true,
                defaultValue: null);

            // ── Step 4: Point every existing Location row to İstanbul (Id = 1) ──
            //    You can update individual rows to the correct city later via SSMS.
            migrationBuilder.Sql("UPDATE Locations SET LocationCityId = 1 WHERE LocationCityId IS NULL;");

            // ── Step 5: Make the column NOT NULL now that every row has a value ──
            migrationBuilder.AlterColumn<int>(
                name: "LocationCityId",
                table: "Locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            // ── Step 6: Add FK + index ────────────────────────────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationCityId",
                table: "Locations",
                column: "LocationCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_LocationCities_LocationCityId",
                table: "Locations",
                column: "LocationCityId",
                principalTable: "LocationCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // ── Step 7: Drop the old free-text column ─────────────────────────────
            migrationBuilder.DropColumn(
                name: "LocationCity",
                table: "Locations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ── Restore the string column ───────────────────────────────────────
            migrationBuilder.AddColumn<string>(
                name: "LocationCity",
                table: "Locations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // ── Restore data from the lookup table ──────────────────────────────
            migrationBuilder.Sql(
                @"UPDATE l SET l.LocationCity = lc.Name
                  FROM Locations l
                  INNER JOIN LocationCities lc ON l.LocationCityId = lc.Id;");

            // ── Drop FK / index / column ─────────────────────────────────────────
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_LocationCities_LocationCityId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_LocationCityId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationCityId",
                table: "Locations");

            // ── Drop the lookup table ─────────────────────────────────────────────
            migrationBuilder.DropTable(
                name: "LocationCities");
        }
    }
}
