using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SegmentPricingPolicyAndPriceDecisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Elasticity",
                table: "Segments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxDailyPrice",
                table: "Segments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinDailyPrice",
                table: "Segments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PriceDecisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<int>(type: "int", nullable: false),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Action = table.Column<double>(type: "float", nullable: false),
                    StateKey = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    IsSettled = table.Column<bool>(type: "bit", nullable: false),
                    SettledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RealizedRentals = table.Column<int>(type: "int", nullable: true),
                    RealizedRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Reward = table.Column<double>(type: "float", nullable: true),
                    IsControlGroup = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceDecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceDecisions_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceDecisions_CarHistory",
                table: "PriceDecisions",
                columns: new[] { "CarId", "DecidedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceDecisions_Settlement",
                table: "PriceDecisions",
                columns: new[] { "IsSettled", "DecidedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceDecisions");

            migrationBuilder.DropColumn(
                name: "Elasticity",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "MaxDailyPrice",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "MinDailyPrice",
                table: "Segments");
        }
    }
}
