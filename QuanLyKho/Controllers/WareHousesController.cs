using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager,Storekeeper")]
    public class WareHousesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;

        public string PrimaryTitle = "Warehouse";

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public WareHousesController(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: WareHouses
        /// <summary>
        /// Action trả về View danh sách tất cả các kho trong hệ thống
        /// </summary>
        public async Task<IActionResult> Index(string filter = "All")
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (_context.WareHouses == null)
                return Problem("Entity set 'AppDbContext.WareHouses'  is null.");

            var warehouseQuery = _context.WareHouses.AsQueryable();

            if (filter == "Show")
                warehouseQuery = warehouseQuery.Where(w => w.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                warehouseQuery = warehouseQuery.Where(w => w.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await warehouseQuery.ToListAsync());
        }

        // GET: WareHouses/Details/5
        /// <summary>
        /// Action trả về View thông tin chi tiết kho
        /// </summary>
        public async Task<IActionResult> Details(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id == null || _context.WareHouses == null)
            {
                return NotFound();
            }

            var wareHouse = await _context.WareHouses.Include(wh => wh.ProductWareHouses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wareHouse == null)
            {
                return NotFound();
            }
            wareHouse.ProductWareHouses.ForEach(pw =>
            {
                pw.Product = _context.Products.Find(pw.ProductId);
            });

            return View(wareHouse);
        }

        // GET: WareHouses/Create
        /// <summary>
        /// Action trả về View tạo mới kho
        /// </summary>
        public IActionResult Create()
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            return View();
        }

        // POST: WareHouses/Create
        /// <summary>
        /// Action tạo mới kho
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PhoneNumber,Address")] WareHouse wareHouse, WarehouseType type)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                // Xử lý id
                string warehouseId = "";
                if (type == WarehouseType.Warehouse)
                {
                    warehouseId = "KH";
                    var count = await _context.WareHouses.Where(w => w.Id.StartsWith("KH")).CountAsync();
                    wareHouse.Id = warehouseId + (count + 1);
                }
                else if (type == WarehouseType.Store)
                {
                    warehouseId = "CH";
                    var count = await _context.WareHouses.Where(w => w.Id.StartsWith("CH")).CountAsync();
                    wareHouse.Id = warehouseId + (count + 1);
                }
                else
                {
                    return View(wareHouse);
                }

                _context.Add(wareHouse);
                wareHouse.SetCreatedTime();
                var kq = await _context.SaveChangesAsync();

                if (kq > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View(wareHouse);
                }
            }
            return View(wareHouse);
        }

        // GET: WareHouses/Edit/5
        /// <summary>
        /// Action trả về View cập nhật thông tin kho(GET)
        /// </summary>
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id == null || _context.WareHouses == null)
            {
                return NotFound();
            }

            var wareHouse = await _context.WareHouses.FindAsync(id);
            if (wareHouse == null)
            {
                return NotFound();
            }
            return View(wareHouse);
        }

        // POST: WareHouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action cập nhật thông tin kho(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,PhoneNumber,Address")] WareHouse wareHouse)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id != wareHouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    wareHouse.SetUpdatedTime();
                    _context.Update(wareHouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WareHouseExists(wareHouse.Id))
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
            return View(wareHouse);
        }

        /// <summary>
        /// Action xóa kho
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || _context.WareHouses is null)
                    return BadRequest();
                var warehouse = _context.WareHouses.Find(id);
                if (warehouse == null)
                    return NotFound();

                //warehouse.Status = Status.Hide;
                //warehouse.SetUpdatedTime();

                _context.WareHouses.Remove(warehouse);

                var kq = await _context.SaveChangesAsync();

                if (kq == 0)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Action Ẩn/Hiện kho
        /// </summary>
        public async Task<IActionResult> Display(string id)
        {
            var warehouse = await _context.WareHouses.FindAsync(id);
            if (warehouse is null)
                return NotFound();

            warehouse.Status = warehouse.Status.ChangeStatus();
            warehouse.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }

        //// GET: WareHouses/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.WareHouses == null)
        //    {
        //        return NotFound();
        //    }

        //    var wareHouse = await _context.WareHouses
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (wareHouse == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(wareHouse);
        //}

        //// POST: WareHouses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.WareHouses == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.WareHouses'  is null.");
        //    }
        //    var wareHouse = await _context.WareHouses.FindAsync(id);
        //    if (wareHouse != null)
        //    {
        //        _context.WareHouses.Remove(wareHouse);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        /// <summary>
        /// Phương thức kiểm tra kho đã tồn tại trong hệ thống hay chưa
        /// </summary>
        private bool WareHouseExists(string id)
        {
            return (_context.WareHouses?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        /// <summary>
        /// API Action trả về danh sách sản phẩm theo kho dựa trên mã kho
        /// </summary>
        [HttpGet("/api/warehouses")]
        public IActionResult GetProductByWarehouseId(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var products = _productService.GetProductByWarehouseId(id);

            return Json(products);
        }
    }
}
