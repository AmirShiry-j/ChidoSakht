using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_cols_carts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastKnownSpecialPrice",
                table: "CartItems",
                newName: "SpecialPrice_LastKnown");

            migrationBuilder.RenameColumn(
                name: "LastKnownPrice",
                table: "CartItems",
                newName: "Price_LastKnown");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecialPrice_LastKnown",
                table: "CartItems",
                newName: "LastKnownSpecialPrice");

            migrationBuilder.RenameColumn(
                name: "Price_LastKnown",
                table: "CartItems",
                newName: "LastKnownPrice");
        }
    }
}
