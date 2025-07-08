using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Create_SpecificationGroups_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecification_ProductSpecificationGroup_ProductSpecificationGroupId",
                table: "ProductSpecification");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecificationGroup_Products_ProductId",
                table: "ProductSpecificationGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecificationGroup",
                table: "ProductSpecificationGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecification",
                table: "ProductSpecification");

            migrationBuilder.RenameTable(
                name: "ProductSpecificationGroup",
                newName: "ProductSpecificationGroups");

            migrationBuilder.RenameTable(
                name: "ProductSpecification",
                newName: "ProductSpecifications");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecificationGroup_ProductId",
                table: "ProductSpecificationGroups",
                newName: "IX_ProductSpecificationGroups_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecification_ProductSpecificationGroupId",
                table: "ProductSpecifications",
                newName: "IX_ProductSpecifications_ProductSpecificationGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecificationGroups",
                table: "ProductSpecificationGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecifications",
                table: "ProductSpecifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecificationGroups_Products_ProductId",
                table: "ProductSpecificationGroups",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecifications_ProductSpecificationGroups_ProductSpecificationGroupId",
                table: "ProductSpecifications",
                column: "ProductSpecificationGroupId",
                principalTable: "ProductSpecificationGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecificationGroups_Products_ProductId",
                table: "ProductSpecificationGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecifications_ProductSpecificationGroups_ProductSpecificationGroupId",
                table: "ProductSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecifications",
                table: "ProductSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecificationGroups",
                table: "ProductSpecificationGroups");

            migrationBuilder.RenameTable(
                name: "ProductSpecifications",
                newName: "ProductSpecification");

            migrationBuilder.RenameTable(
                name: "ProductSpecificationGroups",
                newName: "ProductSpecificationGroup");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecifications_ProductSpecificationGroupId",
                table: "ProductSpecification",
                newName: "IX_ProductSpecification_ProductSpecificationGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecificationGroups_ProductId",
                table: "ProductSpecificationGroup",
                newName: "IX_ProductSpecificationGroup_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecification",
                table: "ProductSpecification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecificationGroup",
                table: "ProductSpecificationGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecification_ProductSpecificationGroup_ProductSpecificationGroupId",
                table: "ProductSpecification",
                column: "ProductSpecificationGroupId",
                principalTable: "ProductSpecificationGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecificationGroup_Products_ProductId",
                table: "ProductSpecificationGroup",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
