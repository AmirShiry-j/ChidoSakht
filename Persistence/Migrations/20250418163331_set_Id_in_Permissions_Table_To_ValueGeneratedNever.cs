using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class set_Id_in_Permissions_Table_To_ValueGeneratedNever : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // حذف محدودیت کلید خارجی از جدول RolePermissions
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            // حذف کلید اصلی از جدول Permissions
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            // حذف ستون Id از جدول Permissions
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Permissions");

            // افزودن ستون جدید Id بدون ویژگی IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // تعریف مجدد کلید اصلی برای جدول Permissions
            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            // افزودن مجدد محدودیت کلید خارجی به جدول RolePermissions
            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // حذف محدودیت کلید خارجی از جدول RolePermissions
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            // حذف کلید اصلی از جدول Permissions
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            // حذف ستون Id از جدول Permissions
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Permissions");

            // افزودن ستون Id با ویژگی IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // تعریف مجدد کلید اصلی برای جدول Permissions
            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            // افزودن مجدد محدودیت کلید خارجی به جدول RolePermissions
            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
