using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class Fix_name_CategoryDetailConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetailedConfig_Categories_CategoryId",
                table: "CategoryDetailedConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetailedConfig_DetailedConfigs_ConfigId",
                table: "CategoryDetailedConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetailedConfig",
                table: "CategoryDetailedConfig");

            migrationBuilder.RenameTable(
                name: "CategoryDetailedConfig",
                newName: "CategoryDetailedConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetailedConfig_ConfigId",
                table: "CategoryDetailedConfigs",
                newName: "IX_CategoryDetailedConfigs_ConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetailedConfig_CategoryId",
                table: "CategoryDetailedConfigs",
                newName: "IX_CategoryDetailedConfigs_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetailedConfigs",
                table: "CategoryDetailedConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetailedConfigs_Categories_CategoryId",
                table: "CategoryDetailedConfigs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetailedConfigs_DetailedConfigs_ConfigId",
                table: "CategoryDetailedConfigs",
                column: "ConfigId",
                principalTable: "DetailedConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetailedConfigs_Categories_CategoryId",
                table: "CategoryDetailedConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetailedConfigs_DetailedConfigs_ConfigId",
                table: "CategoryDetailedConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetailedConfigs",
                table: "CategoryDetailedConfigs");

            migrationBuilder.RenameTable(
                name: "CategoryDetailedConfigs",
                newName: "CategoryDetailedConfig");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetailedConfigs_ConfigId",
                table: "CategoryDetailedConfig",
                newName: "IX_CategoryDetailedConfig_ConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetailedConfigs_CategoryId",
                table: "CategoryDetailedConfig",
                newName: "IX_CategoryDetailedConfig_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetailedConfig",
                table: "CategoryDetailedConfig",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetailedConfig_Categories_CategoryId",
                table: "CategoryDetailedConfig",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetailedConfig_DetailedConfigs_ConfigId",
                table: "CategoryDetailedConfig",
                column: "ConfigId",
                principalTable: "DetailedConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
