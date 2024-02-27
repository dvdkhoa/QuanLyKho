using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Areas.Admin.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public List<UserAndRoles> Users { get; set; }
        public async Task OnGetAsync()
        {
            var qr = _userManager.Users.Select(u => new UserAndRoles()
            {
                Id = u.Id,
                UserName = u.UserName
            });

            Users = await qr.ToListAsync();
            foreach (var u in Users)
            {
                var roles = (await _userManager.GetRolesAsync(u)).ToArray();
                u.RoleNames = string.Join(',',roles);
            }
        }
        public class UserAndRoles : AppUser
        {
            public string RoleNames { get; set; }
        }
    }
}
