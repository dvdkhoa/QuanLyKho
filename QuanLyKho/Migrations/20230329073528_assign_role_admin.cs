using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class assign_role_admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("UserRoles",
                new string[] { "UserId", "RoleId" },
                new object[] { "960b09bc-7737-40e0-a9ba-708b9b321616", "6e89e82b-2282-4b09-863c-da4b87de248e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
