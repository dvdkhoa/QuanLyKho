using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;
using QuanLyKho.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using System.Net.WebSockets;
using MimeKit.Encodings;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StaffsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStaffService _staffService;

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<StaffsController> _logger;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public StaffsController(AppDbContext context, IStaffService staffService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<StaffsController> logger, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _staffService = staffService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        // GET: Staffs
        /// <summary>
        /// Action trả về View danh sách tất cả nhân viên có trong hệ thống
        /// </summary>
        public async Task<IActionResult> Index(string filter = "All")
        {
            if (_context.Staffs == null)
                return Problem("Entity set 'AppDbContext.Staffs'  is null.");

            var staffQuery = _context.Staffs.AsQueryable();

            if (filter == "Show")
                staffQuery = staffQuery.Where(c => c.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                staffQuery = staffQuery.Where(c => c.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await staffQuery.ToListAsync());
        }

        public async Task<IActionResult> StaffInRoles(string roleName)
        {
            var users = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
            var staffs = _context.Staffs.Where(s => users.Contains(s.UserId));
            ViewData["RoleName"] = roleName;
            return View("Index", staffs);
        }

        // GET: Staffs/Details/5
        /// <summary>
        /// Action trả về View thông tin chi tiết của nhân viên
        /// </summary>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .Include(s => s.User)
                .Include(s => s.WareHouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        /// <summary>
        /// Action trả về View tạo mới nhân viên(GET)
        /// </summary>
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");

            var roles = _roleManager.Roles.ToList();
            roles.RemoveAll(role => role.NormalizedName == "ADMIN" || role.NormalizedName == "MANAGER");
            ViewData["Roles"] = new SelectList(roles, "Name", "Name");

            return View();
        }

        // POST: Staffs/Create
        /// <summary>
        /// Action tạo mới nhân viên(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff, string role, IFormFile? newImage) //[Bind("Name,DateOfBirth,Gender, Email, PhoneNumber,Address,StartDay,WareHouseId,UserId, ")] 
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Remove("Id");

                    if (ModelState.IsValid)
                    {
                        Staff newStaff = await _staffService.CreateStaff(staff);
                        if (newImage != null)
                        {
                            var imagePath = await CloudinaryHelper.UploadFileToCloudinary(newImage, "Staffs");
                            newStaff.Image = imagePath;
                        }

                        if (newStaff != null)
                        {
                            var userId = await this.CreateUserAsync(staff);
                            if (!string.IsNullOrEmpty(userId))
                            {
                                newStaff.UserId = userId;
                                _context.Update(newStaff);

                                // gán role cho user
                                var user = await _userManager.FindByIdAsync(userId);
                                await _userManager.AddToRoleAsync(user, role);

                                await _context.SaveChangesAsync();

                                transaction.Commit();

                                return RedirectToAction(nameof(Index));
                            }
                            transaction.Rollback();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    transaction.Rollback();
                }
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
                ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", staff.WareHouseId);
                return View(staff);
            }
        }

        // GET: Staffs/Edit/5
        /// <summary>
        /// Action trả về View cập nhật thông tin cho nhân viên(GET)
        /// </summary>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", staff.WareHouseId);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        /// <summary>
        /// Action cập nhật nhân viên(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Staff staff, IFormFile? newImage)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newImage != null)
                    {
                        var imagePath = await CloudinaryHelper.UploadFileToCloudinary(newImage, "Staffs");
                        staff.Image = imagePath;
                    }

                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", staff.WareHouseId);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.Staffs == null)
        //    {
        //        return NotFound();
        //    }

        //    var staff = await _context.Staffs
        //        .Include(s => s.User)
        //        .Include(s => s.WareHouse)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (staff == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(staff);
        //}

        // POST: Staffs/Delete/5
        /// <summary>
        /// Action xóa nhân viên
        /// </summary>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                if (_context.Staffs == null)
                {
                    return Problem("Entity set 'AppDbContext.Staffs'  is null.");
                }
                var staff = await _context.Staffs.FindAsync(id);
                if (staff != null)
                {
                    if (staff.UserId != null)
                    {
                        var user = await _userManager.FindByIdAsync(staff.UserId);
                        var isDelete = await _userManager.DeleteAsync(user);
                        if (!isDelete.Succeeded)
                            return BadRequest();
                    }
                    _context.Staffs.Remove(staff);
                }

                var kq = await _context.SaveChangesAsync();

                if (kq > 0)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Action Ẩn/Hiện nhân viên
        /// </summary>
        public async Task<IActionResult> Display(string id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff is null)
                return NotFound();

            staff.Status = staff.Status.ChangeStatus();
            staff.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }


        /// <summary>
        /// Phương thức kiểm tra nhân viên có tồn tại trong hệ thống hay chưa
        /// </summary>
        private bool StaffExists(string id)
        {
            return (_context.Staffs?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        /// <summary>
        /// Phương thức tạo tài khoản cho nhân viên =&gt; trả về Id tài khoản
        /// </summary>
        public async Task<string> CreateUserAsync(Staff staff)
        {
            var user = new AppUser { UserName = staff.Email, Email = staff.Email };
            var result = await _userManager.CreateAsync(user, "111111");

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackURL = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code, area = "identity" },
                Request.Scheme
                );

                //var callbackURL = $"/Identity/Account/ConfirmEmail?userId={userId}&code={code}";


                await _emailSender.SendEmailAsync(staff.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackURL)}'>clicking here</a>.");

                return userId;
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return null;
        }
    }



}
