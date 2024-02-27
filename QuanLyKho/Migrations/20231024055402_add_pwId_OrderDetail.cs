using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class add_pwId_OrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductWarehouseId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductWarehouseId",
                table: "OrderDetails",
                column: "ProductWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductWareHouses_ProductWarehouseId",
                table: "OrderDetails",
                column: "ProductWarehouseId",
                principalTable: "ProductWareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductWareHouses_ProductWarehouseId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductWarehouseId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductWarehouseId",
                table: "OrderDetails");
        }
    }
}
