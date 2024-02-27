using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: News
        /// <summary>
        /// Action trả về View danh sách tất cả tin tức có trong hệ thống
        /// </summary>
        public async Task<IActionResult> Index(string filter = "All")
        {
            if (_context.News == null)
                return Problem("Entity set 'AppDbContext.News'  is null.");

            var newQuery = _context.News.AsQueryable();

            if (filter == "Show")
                newQuery = newQuery.Where(c => c.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                newQuery = newQuery.Where(c => c.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await newQuery.ToListAsync());
        }

        // GET: News/Details/5
        /// <summary>
        /// Action trả về View thông tin chi tiết tin tức
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var @new = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }

        // GET: News/Create
        /// <summary>
        /// Action trả về View tạo mới tin tức(GET)
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        /// <summary>
        /// Action tạo mới tin tức(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Description")] New @new, IFormFile? inputImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (inputImage != null)
                    {
                        @new.Image = await CloudinaryHelper.UploadFileToCloudinary(inputImage, "News");
                    }

                    _context.Add(@new);
                    @new.Status = Status.Show;
                    @new.SetCreatedTime();

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(@new);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: News/Edit/5
        /// <summary>
        /// Action trả về View cập nhật tin tức(GET)
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var @new = await _context.News.FindAsync(id);
            if (@new == null)
            {
                return NotFound();
            }
            return View(@new);
        }

        // POST: News/Edit/5
        /// <summary>
        /// Action cập nhật tin tức(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Content,Image,CreatedTime,Status")] New @new, IFormFile? inputImage)
        {
            try
            {
                if (id != @new.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (inputImage != null)
                        {
                            if (@new.Image != null) // Đã có icon trước đó rồi => xóa icon cũ => khúc này làm sau :v
                            {
                                bool isDelete = await CloudinaryHelper.DeteleImage(@new.Image, "News");

                            }
                            var imagePath = await CloudinaryHelper.UploadFileToCloudinary(inputImage, "News");
                            @new.Image = imagePath;
                        }

                        _context.Update(@new);
                        @new.SetUpdatedTime();

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NewExists(@new.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Details), @new);
                }
                return View(@new);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction(nameof(Details), @new);
            }
        }

        // POST: News/Delete/5
        /// <summary>
        /// Action xóa tin tức
        /// </summary>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.News == null)
                {
                    return Problem("Entity set 'AppDbContext.News'  is null.");
                }
                var @new = await _context.News.FindAsync(id);
                if (@new != null)
                {
                    if (@new.Image != null)
                    {
                        var isDeleted = await CloudinaryHelper.DeteleImage(@new.Image, "News");
                    }
                    _context.News.Remove(@new);
                }

                int kq = await _context.SaveChangesAsync();

                return kq > 0 ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Action Ẩn/Hiện tin tức
        /// </summary>
        public async Task<IActionResult> Display(int id)
        {
            var @new = await _context.News.FindAsync(id);
            if (@new is null)
                return NotFound();

            @new.Status = @new.Status.ChangeStatus();
            @new.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }


        /// <summary>
        /// Phương thức kiểm tra tin tức đã tồn tại trong hệ thống hay chưa
        /// </summary>
        private bool NewExists(int id)
        {
            return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
