using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using QuanLyKho.Models.Entities;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class seed_admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var user = new AppUser
            {
                Email = "dangkhoa10052000@gmail.com",
                NormalizedEmail = "dangkhoa10052000@gmail.com",
                UserName = "admin",
                NormalizedUserName = "admin",
                PhoneNumber = "0989337410",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var id = Guid.NewGuid().ToString();

            var hasher = new PasswordHasher<AppUser>();

            var passwordHash = hasher.HashPassword(user, "111111");


            migrationBuilder.InsertData("Users",
                new string[] {"Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                new object[] { id, "admin", "admin", "dangkhoa10052000@gmail.com", "dangkhoa10052000@gmail.com", true, passwordHash, false, false, false, 10 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
