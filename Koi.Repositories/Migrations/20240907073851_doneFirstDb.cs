using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class doneFirstDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsignmentForNurtures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    KoiFishId = table.Column<int>(type: "int", nullable: false),
                    ConsignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceByDayDeale = table.Column<long>(type: "bigint", nullable: false),
                    InspectionRequired = table.Column<bool>(type: "bit", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsignmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ConsignmentForNurtures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsignmentForNurtures_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsignmentForNurtures_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsignmentForNurtures_KoiFishs_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsignmentForSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    KoiFishId = table.Column<int>(type: "int", nullable: false),
                    ConsignmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceDealed = table.Column<long>(type: "bigint", nullable: false),
                    InspectionRequired = table.Column<bool>(type: "bit", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsignmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ConsignmentForSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsignmentForSales_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsignmentForSales_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsignmentForSales_KoiFishs_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForNurtures_CustomerId",
                table: "ConsignmentForNurtures",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForNurtures_KoiFishId",
                table: "ConsignmentForNurtures",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForNurtures_StaffId",
                table: "ConsignmentForNurtures",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForSales_CustomerId",
                table: "ConsignmentForSales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForSales_KoiFishId",
                table: "ConsignmentForSales",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentForSales_StaffId",
                table: "ConsignmentForSales",
                column: "StaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsignmentForNurtures");

            migrationBuilder.DropTable(
                name: "ConsignmentForSales");
        }
    }
}
