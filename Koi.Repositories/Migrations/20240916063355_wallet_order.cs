using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class wallet_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WalletId",
                table: "Orders",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Wallets_WalletId",
                table: "Orders",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Wallets_WalletId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WalletId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Orders");
        }
    }
}
