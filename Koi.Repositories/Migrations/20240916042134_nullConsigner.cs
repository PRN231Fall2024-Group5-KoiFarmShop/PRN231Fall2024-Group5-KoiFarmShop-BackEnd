using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class nullConsigner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KoiFishs_AspNetUsers_ConsignedBy",
                table: "KoiFishs");

            migrationBuilder.AlterColumn<int>(
                name: "ConsignedBy",
                table: "KoiFishs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_KoiFishs_AspNetUsers_ConsignedBy",
                table: "KoiFishs",
                column: "ConsignedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KoiFishs_AspNetUsers_ConsignedBy",
                table: "KoiFishs");

            migrationBuilder.AlterColumn<int>(
                name: "ConsignedBy",
                table: "KoiFishs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiFishs_AspNetUsers_ConsignedBy",
                table: "KoiFishs",
                column: "ConsignedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
