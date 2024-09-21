using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class tienchi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KoiDiaries_AspNetUsers_UserId",
                table: "KoiDiaries");

            migrationBuilder.DropIndex(
                name: "IX_KoiDiaries_UserId",
                table: "KoiDiaries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "KoiDiaries");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "KoiDiaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "KoiDiaries");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "KoiDiaries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KoiDiaries_UserId",
                table: "KoiDiaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KoiDiaries_AspNetUsers_UserId",
                table: "KoiDiaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
