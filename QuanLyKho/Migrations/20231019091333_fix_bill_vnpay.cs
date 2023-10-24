using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class fix_bill_vnpay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VnPay_OrderId",
                table: "VnPay");

            migrationBuilder.DropIndex(
                name: "IX_Bill_OrderId",
                table: "Bill");

            migrationBuilder.CreateIndex(
                name: "IX_VnPay_OrderId",
                table: "VnPay",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_OrderId",
                table: "Bill",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VnPay_OrderId",
                table: "VnPay");

            migrationBuilder.DropIndex(
                name: "IX_Bill_OrderId",
                table: "Bill");

            migrationBuilder.CreateIndex(
                name: "IX_VnPay_OrderId",
                table: "VnPay",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_OrderId",
                table: "Bill",
                column: "OrderId");
        }
    }
}
