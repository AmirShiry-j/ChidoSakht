using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class for_error : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProduct_Products_ProductId",
                table: "RelatedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProduct_Products_RelatedProductId",
                table: "RelatedProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedProduct",
                table: "RelatedProduct");

            migrationBuilder.RenameTable(
                name: "RelatedProduct",
                newName: "RelatedProducts");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedProduct_RelatedProductId",
                table: "RelatedProducts",
                newName: "IX_RelatedProducts_RelatedProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedProducts",
                table: "RelatedProducts",
                columns: new[] { "ProductId", "RelatedProductId" });

            migrationBuilder.CreateTable(
                name: "ProductSpecificationGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecificationGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationGroup_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductSpecificationGroupId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecification_ProductSpecificationGroup_ProductSpecificationGroupId",
                        column: x => x.ProductSpecificationGroupId,
                        principalTable: "ProductSpecificationGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecification_ProductSpecificationGroupId",
                table: "ProductSpecification",
                column: "ProductSpecificationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationGroup_ProductId",
                table: "ProductSpecificationGroup",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Products_ProductId",
                table: "RelatedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Products_ProductId",
                table: "RelatedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductId",
                table: "RelatedProducts");

            migrationBuilder.DropTable(
                name: "ProductSpecification");

            migrationBuilder.DropTable(
                name: "ProductSpecificationGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedProducts",
                table: "RelatedProducts");

            migrationBuilder.RenameTable(
                name: "RelatedProducts",
                newName: "RelatedProduct");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProduct",
                newName: "IX_RelatedProduct_RelatedProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedProduct",
                table: "RelatedProduct",
                columns: new[] { "ProductId", "RelatedProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProduct_Products_ProductId",
                table: "RelatedProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProduct_Products_RelatedProductId",
                table: "RelatedProduct",
                column: "RelatedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
