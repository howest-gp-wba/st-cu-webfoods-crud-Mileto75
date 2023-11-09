using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wba.WebFoods.Web.Migrations
{
    public partial class AddPropertiesRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductProperty",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    PropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProperty", x => new { x.ProductsId, x.PropertiesId });
                    table.ForeignKey(
                        name: "FK_ProductProperty_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProperty_Properties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProperty_PropertiesId",
                table: "ProductProperty",
                column: "PropertiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProperty");
        }
    }
}
