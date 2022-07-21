using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddingSupplierInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierInfoId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "supplierInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_supplierInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierInfoId",
                table: "Products",
                column: "SupplierInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_supplierInfos_UserId",
                table: "supplierInfos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_supplierInfos_SupplierInfoId",
                table: "Products",
                column: "SupplierInfoId",
                principalTable: "supplierInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_supplierInfos_SupplierInfoId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "supplierInfos");

            migrationBuilder.DropIndex(
                name: "IX_Products_SupplierInfoId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SupplierInfoId",
                table: "Products");
        }
    }
}
