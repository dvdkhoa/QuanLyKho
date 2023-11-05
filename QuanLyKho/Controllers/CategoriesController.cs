using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Storekeeper")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public string PrimaryTitle = "Categories";


        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        /// <summary>
        /// Action trả về view danh sách danh mục
        /// </summary>
        public async Task<IActionResult> Index(string filter = "All")
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (_context.Categories == null)
                return Problem("Entity set 'AppDbContext.Categories'  is null.");

            var categoryQuery = _context.Categories.AsQueryable();

            if (filter == "Show")
                categoryQuery = categoryQuery.Where(c => c.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                categoryQuery = categoryQuery.Where(c => c.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await categoryQuery.ToListAsync());
        }

        // GET: Categories/Details/5
        /// <summary>
        /// Action trả về view chi tiết danh mục
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.Include(c => c.CategoryDetailedConfigs).ThenInclude(c => c.DetailedConfig).Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        /// <summary>
        /// Action trả về view tạo danh mục(GET)
        /// </summary>
        public IActionResult Create()
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action tạo mới danh mục(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Status")] Category category, IFormFile iconFile)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (ModelState.IsValid)
            {
                // Status mặc định là Show
                category.Status = Status.Show;

                if (iconFile != null)
                {
                    var imagePath = await CloudinaryHelper.UploadFileToCloudinary(iconFile, "Categories");
                    category.Icon = imagePath;
                }

                category.SetCreatedTime();

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        /// <summary>
        /// Action trả về view cập nhật thông tin danh mục(GET)
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            var selectedValues = _context.CategoryDetailedConfigs.Where(cdc => cdc.CategoryId == id).Select(cdc => cdc.ConfigId).ToList();

            if (category == null)
            {
                return NotFound();
            }

            ViewData["Configurations"] = _context.DetailedConfigs;
            ViewData["SelectedValues"] = selectedValues;

            return View(category);
        }

        // POST: Categories/Edit/5
        /// <summary>
        /// Action cập nhật thông tin danh mục(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Icon,Status")] Category category, int[] configs, IFormFile? iconFile)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (iconFile != null)
                    {
                        if (category.Icon != null) // Đã có icon trước đó rồi => xóa icon cũ => khúc này làm sau :v
                        {
                            bool isDelete = await CloudinaryHelper.DeteleImage(category.Icon, "Categories");

                        }
                        var imagePath = await CloudinaryHelper.UploadFileToCloudinary(iconFile, "Categories");
                        category.Icon = imagePath;
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync(); // Cập nhật thông tin cơ bản của category

                    var oldConfigIds = await _context.CategoryDetailedConfigs.Where(cdc => cdc.CategoryId == category.Id).Select(cdc => cdc.ConfigId).ToListAsync();

                    var addConfigIds = new List<int>();
                    var removeConfigIds = new List<int>();

                    foreach (var configId in configs)
                    {
                        if (!oldConfigIds.Contains(configId)) // không chứa trong list cũ thì thêm vào ds cần thêm
                        {
                            addConfigIds.Add(configId); // thêm vào để xíu xử lý
                        }// ngược lại => đã chứa trong list cũ => thì thôi :v => không làm gì cả
                    }
                    foreach (var configId in oldConfigIds)
                    {
                        if (!configs.Contains(configId)) // không chứa trong list mới thì thêm vào ds cần xóa
                        {
                            removeConfigIds.Add(configId);
                            // Xóa productDetailedConfig luôn
                            var productConfig = await _context.ProductDetailedConfigs.Include(pdc => pdc.Product).Where(pdc => pdc.ConfigId == configId && pdc.Product.CategoryId == category.Id).FirstOrDefaultAsync();
                            if (productConfig != null)
                                _context.Remove(productConfig);
                        }
                    }

                    // Duyệt 2 ds cần thêm và cần xóa
                    foreach (var item in addConfigIds)
                    {
                        var cdc = new CategoryDetailedConfig
                        {
                            CategoryId = category.Id,
                            ConfigId = item,
                        };
                        await _context.CategoryDetailedConfigs.AddAsync(cdc);
                        cdc.SetUpdatedTime();
                    }

                    foreach (var item in removeConfigIds)
                    {
                        var cdc = await _context.CategoryDetailedConfigs.Where(cdc => cdc.ConfigId == item && cdc.CategoryId == category.Id).FirstOrDefaultAsync();
                        if (cdc != null)
                            _context.CategoryDetailedConfigs.Remove(cdc);
                    }
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), category);
            }

            var selectedValues = _context.CategoryDetailedConfigs.Where(cdc => cdc.CategoryId == id).Select(cdc => cdc.ConfigId).ToList();

            ViewData["Configurations"] = _context.DetailedConfigs;
            ViewData["SelectedValues"] = selectedValues;
            return View(category);
        }


        /// <summary>
        /// Action xóa danh mục
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id is null || _context.Categories == null)
                    return NotFound();

                var category = _context.Categories.Find(id);
                if (category == null)
                    return NotFound();

                _context.Categories.Remove(category);

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


        //// GET: Categories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    ViewData["PrimaryTitle"] = PrimaryTitle;

        //    if (id == null || _context.Categories == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Categories
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Categories == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Categories'  is null.");
        //    }
        //    var category = await _context.Categories.FindAsync(id);
        //    if (category != null)
        //    {
        //        //_context.Categories.Remove(category);
        //        category.Status = Status.Hide;
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}



        /// <summary>
        /// Action ẩn/hiện danh mục
        /// </summary>
        public async Task<IActionResult> Display(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                return NotFound();

            category.Status = category.Status.ChangeStatus();
            category.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }

        /// <summary>
        /// Phương thức kiểm tra danh mục tồn tại hay chưa
        /// </summary>
        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
