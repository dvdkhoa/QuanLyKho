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

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StaffsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStaffService _staffService;

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<StaffsController> _logger;
        private readonly IEmailSender _emailSender;

        public StaffsController(AppDbContext context, IStaffService staffService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<StaffsController> logger, IEmailSender emailSender)
        {
            _context = context;
            _staffService = staffService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: Staffs
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

        // GET: Staffs/Details/5
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
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff) //[Bind("Name,DateOfBirth,Gender, Email, PhoneNumber,Address,StartDay,WareHouseId,UserId, ")] 
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                ModelState.Remove("Id");

                if (ModelState.IsValid)
                {
                    bool result = await _staffService.CreateStaff(staff);

                    if (result)
                    {
                        var result_CreateUser = await this.CreateUserAsync(staff);
                        if (result_CreateUser)
                        {
                            transaction.Commit();
                            return RedirectToAction(nameof(Index));
                        }
                        transaction.Rollback();
                    }
                }
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
                ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", staff.WareHouseId);
                return View(staff);
            }
        }

        // GET: Staffs/Edit/5
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
        public async Task<IActionResult> Delete(string id)
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

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'AppDbContext.Staffs'  is null.");
            }
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Display(string id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff is null)
                return NotFound();

            staff.Status = staff.Status.ChangeStatus();
            staff.UpdateTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }


        private bool StaffExists(string id)
        {
            return (_context.Staffs?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<bool> CreateUserAsync(Staff staff)
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

                return true;
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return false;
        }
    }
}
