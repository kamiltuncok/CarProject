using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MoveIdentityNumberDropAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. First add the new target column
            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Customers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            // 2. Safely port the existing data to the new base table column
            migrationBuilder.Sql(
                "UPDATE Customers SET IdentityNumber = (SELECT IdentityNumber FROM IndividualCustomers WHERE IndividualCustomers.Id = Customers.Id) WHERE EXISTS (SELECT 1 FROM IndividualCustomers WHERE IndividualCustomers.Id = Customers.Id)");

            // 3. Drop the old columns
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "IndividualCustomers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. First add the target column back
            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "IndividualCustomers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            // 2. Port the data back
            migrationBuilder.Sql(
                "UPDATE IndividualCustomers SET IdentityNumber = (SELECT IdentityNumber FROM Customers WHERE Customers.Id = IndividualCustomers.Id)");

            // 3. Drop the reversed column
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
