using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BannersController : Controller
    {
        private readonly AppDbContext _context;

        public BannersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Banners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Banner banner, IFormFile inputImage)
        {
            if (ModelState.IsValid)
            {
                if (inputImage != null)
                {
                    var path = await CloudinaryHelper.UploadFileToCloudinary(inputImage, "Banners");
                    banner.Path = path;
                }
                await _context.AddAsync(banner);
                var kq = await _context.SaveChangesAsync();
                if (kq > 0)
                    return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            var banner = _context.Banners.Find(id);

            return View(banner);
        }

        public IActionResult Edit(int id)
        {
            var banner = _context.Banners.Find(id);
            if (banner == null)
                return NotFound();
            return View(banner);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Banner banner, IFormFile? inputImage)
        {
            try
            {
                if (inputImage != null)
                {
                    var isDelete = await CloudinaryHelper.DeteleImage(banner.Path, "Banners");

                    if (isDelete)
                    {
                        banner.Path = await CloudinaryHelper.UploadFileToCloudinary(inputImage, "Banners");
                    }
                }
                _context.Banners.Update(banner);
                await _context.SaveChangesAsync();
                return View(nameof(Details), banner);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner is null)
                return NotFound();

            var kq = await CloudinaryHelper.DeteleImage(banner.Path, "Banners");
            _context.Remove(banner);
            await _context.SaveChangesAsync();

            return Ok("Delete successfully");
        }
    }
}
