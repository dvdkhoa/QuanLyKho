using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: News
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] New @new, IFormFile? inputImage)
        {
            if (ModelState.IsValid)
            {
                if(inputImage != null)
                {
                    @new.Image = await CloudinaryHelper.UploadFileToCloudinary(inputImage, "News");
                }

                _context.Add(@new);
                @new.Status = Status.Show;
                @new.UpdateTime();

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@new);
        }

        // GET: News/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content, Image,Status")] New @new, IFormFile? inputImage)
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
                    @new.UpdateTime();

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

        // GET: ư/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
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


        public async Task<IActionResult> Display(int id)
        {
            var @new = await _context.News.FindAsync(id);
            if (@new is null)
                return NotFound();

            @new.Status = @new.Status.ChangeStatus();
            @new.UpdateTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }


        private bool NewExists(int id)
        {
            return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
