using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.ProjectModel;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;
using DinkToPdf;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using DinkToPdf.Contracts;

namespace QuanLyKho.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;
        private readonly IConverter _converter;

        public ReceiptsController(AppDbContext context, IReceiptService receiptService, IProductService productService, IConverter converter)
        {
            _context = context;
            _receiptService = receiptService;
            _productService = productService;
            _converter = converter;
        }

        // GET: Receipts
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Receipts.Where(receipt => receipt.Status == Status.Show).Include(r => r.Staff).Include(r => r.WareHouse);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Receipts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Receipts == null)
            {
                return NotFound();
            }

            var receiptInfo = await _receiptService.GetFullInfoReceipt(id.Value);

            if (receiptInfo == null)
            {
                return NotFound();
            }

            return View(receiptInfo);
        }

        // GET: Receipts/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id");
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");

            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreated,Type,WareHouseId,StaffId")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receipt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id", receipt.StaffId);
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", receipt.WareHouseId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View(receipt);
        }

        // GET: Receipts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Receipts == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id", receipt.StaffId);
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", receipt.WareHouseId);
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,Type,WareHouseId,StaffId")] Receipt receipt)
        {
            if (id != receipt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.Id))
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
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id", receipt.StaffId);
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id", receipt.WareHouseId);
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Receipts == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts
                .Include(r => r.Staff)
                .Include(r => r.WareHouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Receipts == null)
            {
                return Problem("Entity set 'AppDbContext.Receipts'  is null.");
            }
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt != null)
            {
                _context.Receipts.Remove(receipt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(int id)
        {
            return (_context.Receipts?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReceipt(Receipt receipt, List<string>? ProductIds, List<int>? productQuantitys, List<NewProductModel>? newProducts)
        {
            if (receipt == null || (ProductIds is null && newProducts is null))
                return BadRequest();


            receipt.DateCreated = DateTime.Now;
            List<ReceiptDetail> _receiptDetails = new List<ReceiptDetail>();


            if (receipt.Type == ReceiptType.Import) // Phiếu nhập
            {
                if (ProductIds != null && productQuantitys != null)
                {
                    if (ProductIds.Count == productQuantitys.Count)
                    {
                        Dictionary<string, int> productsInStock = new Dictionary<string, int>();

                        for (int i = 0; i < ProductIds.Count; i++)
                        {
                            var receiptDetail = new ReceiptDetail
                            {
                                ProductId = ProductIds[i],
                                Quantity = productQuantitys[i],
                            };

                            _receiptDetails.Add(receiptDetail);
                        }
                    }
                }

                if (newProducts != null)
                {
                    var products = newProducts.Select(newProduct => new Product { Id = newProduct.Id, Name = newProduct.Name, Price = newProduct.Price }).ToList();
                    await _context.Products.AddRangeAsync(products);

                    bool add_results = await _context.SaveChangesAsync() > 0;
                    if (add_results)
                    {
                        var receiptDetails = newProducts.Select(p =>
                                                new ReceiptDetail
                                                { ProductId = p.Id, Quantity = p.Quantity }).ToList();

                        _receiptDetails.AddRange(receiptDetails);
                    }
                }

                _context.Add(receipt);
                _context.SaveChanges();

                List<ProductWareHouse> _productWareHouses = new List<ProductWareHouse>();


                _receiptDetails.ForEach(i =>
                {
                    i.ReceiptId = receipt.Id;

                    // Thực hiện cập nhật ProductWarehouse
                    var productWareHouseExist = _context.ProductWareHouses.Where(pw => pw.WareHouseId == receipt.WareHouseId).FirstOrDefault(pw => pw.ProductId == i.ProductId);
                    if (productWareHouseExist != null) // Kiểm tra xem trong ProductWarehouse đã tồn tại sản phẩm này chưa, nếu đã tồn tại thì cập nhật lại quantity
                    {
                        productWareHouseExist.Quantity += i.Quantity;
                    }
                    else // Nếu chưa tồn tại thì tạo mới 1 record ProductWarehouse
                    {
                        var pw = new ProductWareHouse { ProductId = i.ProductId, Quantity = i.Quantity, WareHouseId = receipt.WareHouseId };
                        _productWareHouses.Add(pw);
                    }
                });
                await _context.ReceiptDetails.AddRangeAsync(_receiptDetails); // insert các ReceiptDetail

                // Thực hiện insert các ProductWarehouses mới
                await _context.ProductWareHouses.AddRangeAsync(_productWareHouses);

                await _context.SaveChangesAsync(); // Lưu 

                return RedirectToAction(nameof(Index));

            }
            else if (receipt.Type == ReceiptType.Export) // Phiếu xuất
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    if (ProductIds != null && productQuantitys != null)
                    {
                        if (ProductIds.Count == productQuantitys.Count)
                        {
                            Dictionary<string, int> productsInStock = new Dictionary<string, int>();

                            for (int i = 0; i < ProductIds.Count; i++)
                            {
                                productsInStock.Add(ProductIds[i], productQuantitys[i]);
                            }

                            var receiptDetails = productsInStock.Select(i => new ReceiptDetail
                            {
                                ProductId = i.Key,
                                Quantity = i.Value,
                            });

                            _receiptDetails.AddRange(receiptDetails);
                        }
                    }
                    _context.Add(receipt);
                    _context.SaveChanges();

                    List<ProductWareHouse> _productWareHouses = new List<ProductWareHouse>();

                    foreach (var i in _receiptDetails)
                    {
                        i.ReceiptId = receipt.Id;

                        // Check số lượng hợp lệ hay không
                        var isValid = _productService.CheckQuantityValid(receipt.WareHouseId, i.ProductId, i.Quantity);
                        if (!isValid)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("", $"Product {i.ProductId} has quantity less than quantity you want export");
                            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id");
                            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                            ViewData["ProductId"] = new MultiSelectList(_context.Products, "Id", "Id", ProductIds);
                            return View("Create", receipt);
                        }
                        // Thực hiện cập nhật ProductWarehouse
                        var productWareHouseExist = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == i.ProductId);
                        if (productWareHouseExist != null) // Kiểm tra xem trong ProductWarehouse đã tồn tại sản phẩm này chưa, nếu đã tồn tại thì cập nhật lại quantity
                        {
                            productWareHouseExist.Quantity -= i.Quantity;
                        }
                    }

                    await _context.ReceiptDetails.AddRangeAsync(_receiptDetails);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id");
                ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");

                return View("Create", receipt);
            }
        }
        public async Task<IActionResult> RenderReceipt(int id)
        {
            if (id == 0)
                return BadRequest();
            var receipt = await _context.Receipts
                                        .Include(r => r.Staff)
                                        .Include(r => r.WareHouse)
                                        .FirstOrDefaultAsync(r => r.Id == id);
            if (receipt == null)
                return BadRequest();

            receipt.ReceiptDetails = _context.ReceiptDetails.Include(d => d.Product).Where(rd => rd.ReceiptId == id).ToList();

            return View(receipt);
        }

        public async Task<IActionResult> Test(int id)
        {

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4Plus,
                Margins = new MarginSettings() { Top = 10, Bottom=20, Left=20, Right=20 },
                },
                Objects = {
                            new ObjectSettings()
                            {
                                Page = "https://localhost:7055/Receipts/RenderReceipt/"+id,
                                PagesCount = true,
                                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = "https://localhost:7055/lib/bootstrap/dist/css/bootstrap.css" }
                            },

                        }
            };
            var bytes = _converter.Convert(doc);


            var stream = new MemoryStream(bytes);

            return File(stream, "application/pdf");
        }
    }
}
