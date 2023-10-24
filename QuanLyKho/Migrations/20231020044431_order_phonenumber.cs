using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class order_phonenumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Orders_OrderId1",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_VnPays_Orders_OrderId1",
                table: "VnPays");

            migrationBuilder.DropIndex(
                name: "IX_VnPays_OrderId1",
                table: "VnPays");

            migrationBuilder.DropIndex(
                name: "IX_Bills_OrderId1",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "VnPays");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Bills");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "VnPays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VnPays_OrderId1",
                table: "VnPays",
                column: "OrderId1",
                unique: true,
                filter: "[OrderId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_OrderId1",
                table: "Bills",
                column: "OrderId1",
                unique: true,
                filter: "[OrderId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Orders_OrderId1",
                table: "Bills",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VnPays_Orders_OrderId1",
                table: "VnPays",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
