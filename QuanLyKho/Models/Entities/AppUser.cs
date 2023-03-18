using Microsoft.AspNetCore.Identity;

namespace QuanLyKho.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public Staff Staff { get; set; }
        public int StaffId { get; set; }
    }
}
