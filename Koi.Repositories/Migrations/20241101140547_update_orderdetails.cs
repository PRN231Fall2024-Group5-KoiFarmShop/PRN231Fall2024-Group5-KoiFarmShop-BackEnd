using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class update_orderdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NurtureStatus",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "ShippingStatus",
                table: "OrderDetails",
                newName: "Note");

            migrationBuilder.AddColumn<long>(
                name: "ConsignmentCost",
                table: "OrderDetails",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsignmentCost",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "OrderDetails",
                newName: "ShippingStatus");

            migrationBuilder.AddColumn<string>(
                name: "NurtureStatus",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
