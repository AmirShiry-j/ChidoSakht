using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_relation_OnDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductAttributeValues_ProductAttributeValueId",
                table: "ProductVariantAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductVariants_ProductVariantId",
                table: "ProductVariantAttributeValues");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductAttributeValues_ProductAttributeValueId",
                table: "ProductVariantAttributeValues",
                column: "ProductAttributeValueId",
                principalTable: "ProductAttributeValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductVariants_ProductVariantId",
                table: "ProductVariantAttributeValues",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductAttributeValues_ProductAttributeValueId",
                table: "ProductVariantAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductVariants_ProductVariantId",
                table: "ProductVariantAttributeValues");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductAttributeValues_ProductAttributeValueId",
                table: "ProductVariantAttributeValues",
                column: "ProductAttributeValueId",
                principalTable: "ProductAttributeValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributeValues_ProductVariants_ProductVariantId",
                table: "ProductVariantAttributeValues",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");
        }
    }
}
