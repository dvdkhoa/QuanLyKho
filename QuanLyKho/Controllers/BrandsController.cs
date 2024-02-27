using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using System.Reflection;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager,Storekeeper")]
    public class BrandsController : Controller
    {
        private readonly AppDbContext _context;


        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filter = "All")
        {

            if (_context.Brands == null)
                return Problem("Entity set 'AppDbContext.Brands'  is null.");

            var brandQuery = _context.Brands.AsQueryable();

            if (filter == "Show")
                brandQuery = brandQuery.Where(c => c.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                brandQuery = brandQuery.Where(c => c.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await brandQuery.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand, string[] categories, IFormFile newThumbnail)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Remove("Thumbnail");
                    if (ModelState.IsValid)
                    {
                        await _context.Brands.AddAsync(brand);

                        if (newThumbnail != null)
                        {
                            string path = await CloudinaryHelper.UploadFileToCloudinary(newThumbnail, "Brands");
                            brand.Thumbnail = path;
                        }
                        brand.Status = Status.Show;
                        brand.SetCreatedTime();
                        var kq = await _context.SaveChangesAsync();

                        if (kq > 0)
                        {
                            if (categories.Length > 0)
                            {
                                kq = 0;
                                var list_categories = categories.Select(int.Parse).ToList();

                                List<CategoryBrand> categoryBrands = list_categories.Select(item => new CategoryBrand
                                {
                                    CategoryId = item,
                                    BrandId = brand.Id
                                }).ToList();

                                await _context.AddRangeAsync(categoryBrands);

                                kq = await _context.SaveChangesAsync();
                                if (kq == 0)
                                {
                                    
                                    transaction.Rollback();
                                    ModelState.AddModelError("", "Insert failed");
                                    return View(brand);
                                }
                            }
                        }
                        transaction.Commit();
                        return RedirectToAction("Details", new { id = brand.Id });
                    }
                    return View(brand);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", ex.Message);
                    return View(brand);
                }
            }
        }


        public IActionResult Details(int id)
        {
            var brand = _context.Brands.Find(id);

            if (brand is null)
                return NotFound();

            brand.CategoryBrands = _context.CategoryBrands.Where(cb => cb.BrandId == id).Include(cb => cb.Category).ToList();
            return View(brand);
        }

        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.Find(id);

            if (brand is null)
                return NotFound();


            var selectedValues = _context.CategoryBrands.Where(cdc => cdc.BrandId == id).Select(cdc => cdc.CategoryId).ToList();

            ViewData["Categories"] = _context.Categories;
            ViewData["SelectedValues"] = selectedValues;

            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Brand brand, int[] newCategories, IFormFile? newThumbnail)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (newThumbnail != null)
                        {
                            if (brand.Thumbnail != null) // Đã có icon trước đó rồi => xóa icon cũ => khúc này làm sau :v
                            {
                                bool isDelete = await CloudinaryHelper.DeteleImage(brand.Thumbnail, "Brands");

                            }
                            var imagePath = await CloudinaryHelper.UploadFileToCloudinary(newThumbnail, "Brands");
                            brand.Thumbnail = imagePath;
                        }

                        brand.SetUpdatedTime();
                        _context.Update(brand);
                        await _context.SaveChangesAsync(); // Cập nhật thông tin cơ bản của brand

                        var oldCateIds = await _context.CategoryBrands.Where(cb => cb.BrandId == brand.Id).Select(cdc => cdc.CategoryId).ToListAsync();

                        var addCateIds = new List<int>();
                        var removeCateIds = new List<int>();

                        foreach (var cateId in newCategories)
                        {
                            if (!oldCateIds.Contains(cateId)) // không chứa trong list cũ thì thêm vào ds cần thêm
                            {
                                addCateIds.Add(cateId); // thêm vào để xíu xử lý
                            }// ngược lại => đã chứa trong list cũ => thì thôi :v => không làm gì cả
                        }
                        foreach (var cateId in oldCateIds)
                        {
                            if (!newCategories.Contains(cateId)) // không chứa trong list mới thì thêm vào ds cần xóa
                            {
                                removeCateIds.Add(cateId);
                                // Xóa CategoryBrand luôn
                                //var categoryBrand = await _context.CategoryBrands.Include(pdc => pdc.Product).Where(pdc => pdc.ConfigId == configId && pdc.Product.brandId == brand.Id).FirstOrDefaultAsync()
                                var categoryBrand = await _context.CategoryBrands.FirstOrDefaultAsync(cb => cb.BrandId == brand.Id && cb.CategoryId == cateId);
                                if (categoryBrand != null)
                                    _context.CategoryBrands.Remove(categoryBrand);
                            }
                        }

                        // Duyệt 2 ds cần thêm và cần xóa
                        foreach (var cateId in addCateIds)
                        {
                            var cb = new CategoryBrand
                            {
                                BrandId = brand.Id,
                                CategoryId = cateId,
                            };
                            await _context.CategoryBrands.AddAsync(cb);
                        }

                        foreach (var cateId in removeCateIds)
                        {
                            var cb = await _context.CategoryBrands.FirstOrDefaultAsync(cb => cb.BrandId == brand.Id && cb.CategoryId == cateId);
                            if (cb != null)
                                _context.CategoryBrands.Remove(cb);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Details), brand);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        await transaction.RollbackAsync();
                    }
                }
            }

            var selectedValues = _context.CategoryBrands.Where(cdc => cdc.BrandId == id).Select(cdc => cdc.CategoryId).ToList();

            ViewData["Categories"] = _context.Categories;
            ViewData["SelectedValues"] = selectedValues;

            return View(brand);
        }

        /// <summary>
        /// Action ẩn/hiện thương hiệu
        /// </summary>
        public async Task<IActionResult> Display(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand is null)
                return NotFound();

            brand.Status = brand.Status.ChangeStatus();
            brand.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var brand = await _context.Brands.FindAsync(id);
                if (brand is null)
                    return NotFound();

                _context.Brands.Remove(brand);
                var kq = await _context.SaveChangesAsync();
                if (kq > 0)
                    return Ok();
                return BadRequest("Delete fail");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}