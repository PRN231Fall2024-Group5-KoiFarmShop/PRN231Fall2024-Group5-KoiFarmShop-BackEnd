using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class add_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodCost",
                table: "ConsignmentForNurtures");

            migrationBuilder.RenameColumn(
                name: "PriceByDay",
                table: "ConsignmentForNurtures",
                newName: "DietCost");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionDate",
                table: "ConsignmentForNurtures",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DietCost",
                table: "ConsignmentForNurtures",
                newName: "PriceByDay");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionDate",
                table: "ConsignmentForNurtures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FoodCost",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: true);
        }
    }
}
