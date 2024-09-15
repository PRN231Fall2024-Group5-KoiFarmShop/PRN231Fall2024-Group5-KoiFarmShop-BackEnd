using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koi.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class test_ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KoiFishKoiBreeds");

            migrationBuilder.CreateTable(
                name: "KoiBreedKoiFish",
                columns: table => new
                {
                    KoiBreedsId = table.Column<int>(type: "int", nullable: false),
                    KoiFishesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiBreedKoiFish", x => new { x.KoiBreedsId, x.KoiFishesId });
                    table.ForeignKey(
                        name: "FK_KoiBreedKoiFish_KoiBreeds_KoiBreedsId",
                        column: x => x.KoiBreedsId,
                        principalTable: "KoiBreeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoiBreedKoiFish_KoiFishs_KoiFishesId",
                        column: x => x.KoiFishesId,
                        principalTable: "KoiFishs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KoiBreedKoiFish_KoiFishesId",
                table: "KoiBreedKoiFish",
                column: "KoiFishesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KoiBreedKoiFish");

            migrationBuilder.CreateTable(
                name: "KoiFishKoiBreeds",
                columns: table => new
                {
                    KoiFishId = table.Column<int>(type: "int", nullable: false),
                    KoiBreedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiFishKoiBreeds", x => new { x.KoiFishId, x.KoiBreedId });
                    table.ForeignKey(
                        name: "FK_KoiFishKoiBreeds_KoiBreeds_KoiBreedId",
                        column: x => x.KoiBreedId,
                        principalTable: "KoiBreeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoiFishKoiBreeds_KoiFishs_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KoiFishKoiBreeds_KoiBreedId",
                table: "KoiFishKoiBreeds",
                column: "KoiBreedId");
        }
    }
}
