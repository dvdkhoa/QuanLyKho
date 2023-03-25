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
    public class CreateModel : RolePageModel
    {
        public CreateModel(AppDbContext context, RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập đầy đủ thông tin !");
                return Page();
            }    

            var role = new IdentityRole() { Name = Input.RoleName };

            var result = await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo thành công vai trò: {role.Name}";
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
            [Display(Name ="Vai trò")]
            [Required(ErrorMessage ="Không được bỏ trống {0}")]
            [StringLength(100,MinimumLength = 2,ErrorMessage ="Độ dài {0} phải từ {2} đến {1} ký tự")]
            public string RoleName { get; set; }
        }
    }
}
