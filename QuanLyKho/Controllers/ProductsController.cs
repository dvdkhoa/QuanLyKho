using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.DTO;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;

namespace QuanLyKho.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        public string PrimaryTitle = "Product";

        public ProductsController(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            var appDbContext = _context.Products.Where(product => product.Status == Status.Show).Include(p => p.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productInfo = _productService.getProductInfo(id);

            if (productInfo == null)
            {
                return NotFound();
            }

            return View(productInfo);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind()] CreateProductModel createProductModel)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (ModelState.IsValid)
            {
                _context.Add(createProductModel.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", createProductModel.Product.CategoryId);
            return View(createProductModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,Status,Price,Unit,Supplier,CategoryId")] Product product)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (string.IsNullOrEmpty(id) || _context.Products is null)
                return BadRequest();

            var product = _context.Products.Find(id);
            if (product is null)
                return NotFound();

            product.Status = Status.Hide;

            await _context.SaveChangesAsync();

            return Ok();
        }


        public IActionResult ImportHistory(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var product = _context.Products.Find(id);
            if (product is null)
                return NotFound();

            var inventoryHistorys = _context.ReceiptDetails.Include(rd => rd.Product).Include(rd => rd.Receipt).Include(rd => rd.Receipt.WareHouse).ToList();

            var model = inventoryHistorys.Select(h => new InventoryHistory { Product = h.Product, WareHouse = h.Receipt.WareHouse, ReceiptDetail = h }).ToList();


            return View(model);
        }


        private bool ProductExists(string id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
