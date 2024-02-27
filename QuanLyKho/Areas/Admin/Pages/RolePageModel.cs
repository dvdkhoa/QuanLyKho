using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Areas.Admin.Pages
{
    public class RolePageModel : PageModel
    {
        protected readonly AppDbContext _context;
        protected readonly RoleManager<IdentityRole> _roleManager;

        [TempData]
        public string StatusMessage { get; set; }

        public RolePageModel(AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _context = context;
        }

    }
}
