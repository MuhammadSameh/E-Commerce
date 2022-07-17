using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddingMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Products_ProductId",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Media",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Media_ProductId",
                table: "Media",
                newName: "IX_Media_InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Inventories_InventoryId",
                table: "Media",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Inventories_InventoryId",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Media",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Media_InventoryId",
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
    }
}
