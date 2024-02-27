using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class EditModel : RolePageModel
    {
        public EditModel(AppDbContext context, RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IdentityRole role { get; set; }

        public async Task OnGet(string roleId)
        {
            role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
                Input = new InputModel() { RoleName = role.Name };
        }

        public async Task<IActionResult> OnPostAsync(string roleId)
        {

            if (string.IsNullOrEmpty(roleId))
                return NotFound("Không tìm thấy Role");
            role = await _roleManager.FindByIdAsync(roleId);
            if(role==null)
                return NotFound("Không tìm thấy Role");

            role.Name = Input.RoleName;

            var result = await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa thay đổi thông tin vai trò thành công!";
                return RedirectToPage("Index");
            }
            else
            {
                result.Errors.ToList().ForEach(err =>
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                });
            }
            return Page();
        }
        public class InputModel
        {
            [Display(Name = "Vai trò")]
            [Required(ErrorMessage = "Không được bỏ trống {0}")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "Độ dài {0} phải từ {2} đến {1} ký tự")]
            public string RoleName { get; set; }
        }
    }
}
