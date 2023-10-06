using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Storekeeper")]
    public class DetailConfigurationController : Controller
    {
        private readonly AppDbContext _context;

        public DetailConfigurationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.DetailedConfigs);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(DetailedConfig detailedConfig, string[] categories)
        {

            await _context.AddAsync(detailedConfig);
            await _context.SaveChangesAsync();



            var list_categories = categories.Select(int.Parse).ToList();

            List<CategoryDetailedConfig> categoryDetaileds = list_categories.Select(item => new CategoryDetailedConfig
            {
                CategoryId = item,
                CreatedTime = DateTime.Now,
                LastUpdated = DateTime.Now,
                ConfigId = detailedConfig.Id,
                Status = Status.Show,
            }).ToList();

            await _context.AddRangeAsync(categoryDetaileds);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = detailedConfig.Id });
        }

        public IActionResult Details(int id)
        {
            var detailModel = _context.DetailedConfigs.Find(id);
            detailModel.CategoryDetailedConfigs = _context.CategoryDetailedConfigs.Where(cdc => cdc.DetailedConfig.Id == id).Include(cdc => cdc.Category).ToList();
            return View(detailModel);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var detailConfig = await _context.DetailedConfigs.FindAsync(id);
  
            return View("Edit", detailConfig);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(DetailedConfig detailedConfig)
        {
            if (detailedConfig == null)
                return NotFound();
            _context.Update(detailedConfig);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", detailedConfig);
        }
    }
}
