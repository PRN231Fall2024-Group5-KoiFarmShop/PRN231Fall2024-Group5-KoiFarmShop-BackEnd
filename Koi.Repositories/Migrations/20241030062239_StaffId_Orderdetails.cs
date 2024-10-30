using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class StaffId_Orderdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_StaffId",
                table: "OrderDetails",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_AspNetUsers_StaffId",
                table: "OrderDetails",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_AspNetUsers_StaffId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_StaffId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "OrderDetails");
        }
    }
}
