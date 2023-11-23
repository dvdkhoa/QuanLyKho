using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Areas.Admin.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public AppUser user{ get; set; }

        public SelectList allRoles { get; set; }

        [BindProperty]
        [Display(Name ="List roles")]
        public string[] RoleNames { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không tìm thấy User có ID: '{_userManager.GetUserId(User)}'.");
            }

            var roles = _roleManager.Roles.Select(r=>r.Name).ToArray(); // Lấy tất cả các role trong hệ thống.

            allRoles = new SelectList(roles);

            // Lấy tất cả các role của User.
            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray<string>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không tìm thấy User có ID: '{_userManager.GetUserId(User)}'.");
            }

            var oldRoles = (await _userManager.GetRolesAsync(user)).ToArray();

            var deleteRoles = oldRoles.Where(r => !RoleNames.Contains(r));
            var insertRoles = RoleNames.Where(r => !oldRoles.Contains(r));

            var removeResult = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if(!removeResult.Succeeded)
            {
                removeResult.Errors.ToList().ForEach(e =>
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                });

                return Page();
            }    

            var addResult = await _userManager.AddToRolesAsync(user, insertRoles);
            if (!addResult.Succeeded)
            {
                addResult.Errors.ToList().ForEach(e =>
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                });

                return Page();
            }

            StatusMessage = $"Bạn vừa cập nhật vai trò(Role) cho User {user.UserName}";
            return RedirectToPage("./Index");
        }
    }
}
