using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class BrandCategoryRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandCategory",
                columns: table => new
                {
                    BrandsBrandId = table.Column<int>(type: "int", nullable: false),
                    CategoriesCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandCategory", x => new { x.BrandsBrandId, x.CategoriesCategoryId });
                    table.ForeignKey(
                        name: "FK_BrandCategory_Brands_BrandsBrandId",
                        column: x => x.BrandsBrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandCategory_Categories_CategoriesCategoryId",
                        column: x => x.CategoriesCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandCategory_CategoriesCategoryId",
                table: "BrandCategory",
                column: "CategoriesCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandCategory");
        }
    }
}
