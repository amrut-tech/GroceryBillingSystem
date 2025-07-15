using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryBillingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMobileNumberToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Customers",
                newName: "MobileNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "Customers",
                newName: "PhoneNumber");
        }
    }
}
