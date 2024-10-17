using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class nullable_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentForNurtures_Diets_DietId",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentForNurtures_PackageCares_PackageCareId",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropTable(
                name: "PackageCares");

            migrationBuilder.DropIndex(
                name: "IX_ConsignmentForNurtures_PackageCareId",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "PackageCareId",
                table: "ConsignmentForNurtures");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DietId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentForNurtures_Diets_DietId",
                table: "ConsignmentForNurtures",
                column: "DietId",
                principalTable: "Diets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentForNurtures_Diets_DietId",
                table: "ConsignmentForNurtures");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DietId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PackageCareId",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PackageCares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodCost = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    LaborCost = table.Column<long>(type: "bigint", nullable: true),
                    MaxSize = table.Column<int>(type: "int", nullable: false),
                    MinSize = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCost = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageCares", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForNurtures_PackageCareId",
                table: "ConsignmentForNurtures",
                column: "PackageCareId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentForNurtures_Diets_DietId",
                table: "ConsignmentForNurtures",
                column: "DietId",
                principalTable: "Diets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentForNurtures_PackageCares_PackageCareId",
                table: "ConsignmentForNurtures",
                column: "PackageCareId",
                principalTable: "PackageCares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
