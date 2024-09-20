using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsConsignmentIncluded",
                table: "Orders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsignmentForNurtureId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NurtureStatus",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingStatus",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "KoiBreeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "KoiDiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoiFishId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiDiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoiDiaries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KoiDiaries_KoiFishs_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ConsignmentForNurtureId",
                table: "OrderDetails",
                column: "ConsignmentForNurtureId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiDiaries_KoiFishId",
                table: "KoiDiaries",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiDiaries_UserId",
                table: "KoiDiaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ConsignmentForNurtures_ConsignmentForNurtureId",
                table: "OrderDetails",
                column: "ConsignmentForNurtureId",
                principalTable: "ConsignmentForNurtures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ConsignmentForNurtures_ConsignmentForNurtureId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "KoiDiaries");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ConsignmentForNurtureId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "IsConsignmentIncluded",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ConsignmentForNurtureId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "NurtureStatus",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShippingStatus",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "KoiBreeds");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "AspNetUsers",
                newName: "ProfilePictureUrl");

            migrationBuilder.AddColumn<int>(
                name: "SubTotal",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
