using Microsoft.EntityFrameworkCore.Migrations;

namespace Pors.Infrastructure.Persistence.Migrations
{
    public partial class add_role_permission_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "RolePermission");

            migrationBuilder.RenameTable(
                name: "RolePermission",
                newName: "RolePermissions");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_Controller",
                table: "RolePermissions",
                newName: "IX_RolePermissions_Controller");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_Action",
                table: "RolePermissions",
                newName: "IX_RolePermissions_Action");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "RolePermission");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermission",
                newName: "IX_RolePermission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_Controller",
                table: "RolePermission",
                newName: "IX_RolePermission_Controller");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_Action",
                table: "RolePermission",
                newName: "IX_RolePermission_Action");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "RolePermission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
