using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Modifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Products_pId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Products_ProductId1",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_ProductId1",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "pId",
                table: "Media",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Media_pId",
                table: "Media",
                newName: "IX_Media_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Products_ProductId",
                table: "Media",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Products_ProductId",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Media",
                newName: "pId");

            migrationBuilder.RenameIndex(
                name: "IX_Media_ProductId",
                table: "Media",
                newName: "IX_Media_pId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_ProductId1",
                table: "Media",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Products_pId",
                table: "Media",
                column: "pId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Products_ProductId1",
                table: "Media",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
