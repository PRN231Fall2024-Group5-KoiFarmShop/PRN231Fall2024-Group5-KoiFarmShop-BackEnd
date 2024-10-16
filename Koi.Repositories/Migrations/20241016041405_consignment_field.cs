using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class consignment_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceByDayDeale",
                table: "ConsignmentForNurtures");

            migrationBuilder.AlterColumn<bool>(
                name: "InspectionRequired",
                table: "ConsignmentForNurtures",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

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
                name: "ActualCost",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyFeedAmount",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FoodCost",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LaborCost",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ConsignmentForNurtures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PriceByDay",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProjectedCost",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "ConsignmentForNurtures",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCost",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "DailyFeedAmount",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "FoodCost",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "LaborCost",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "PriceByDay",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "ProjectedCost",
                table: "ConsignmentForNurtures");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "ConsignmentForNurtures");

            migrationBuilder.AlterColumn<bool>(
                name: "InspectionRequired",
                table: "ConsignmentForNurtures",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionDate",
                table: "ConsignmentForNurtures",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<long>(
                name: "PriceByDayDeale",
                table: "ConsignmentForNurtures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
