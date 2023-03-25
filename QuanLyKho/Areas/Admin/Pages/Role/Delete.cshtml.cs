using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyKho.Areas.Admin.Pages;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;

namespace QuanLyKho.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(AppDbContext context, RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        {
        }

        public IdentityRole Role { get; set; }
        public async Task<IActionResult> OnGetAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return NotFound();
            this.Role = await _roleManager.FindByIdAsync(roleId);
            if (this.Role == null)
                return NotFound();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return NotFound();
            this.Role = await _roleManager.FindByIdAsync(roleId);
            if (this.Role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(this.Role);
            if(result.Succeeded)
            {
                this.StatusMessage = $"Bạn vừa xóa vai trò: {Role.Name}";
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xóa không thành công !");
                return Page();
            }
        }
    }
}
