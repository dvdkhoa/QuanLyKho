using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class add_image_to_staff_customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_Classifications_ClassificationId",
                table: "ProductClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_Products_ProductId",
                table: "ProductClassifications");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_Classifications_ClassificationId",
                table: "ProductClassifications",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_Products_ProductId",
                table: "ProductClassifications",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_Classifications_ClassificationId",
                table: "ProductClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_Products_ProductId",
                table: "ProductClassifications");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Customer");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_Classifications_ClassificationId",
                table: "ProductClassifications",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_Products_ProductId",
                table: "ProductClassifications",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
