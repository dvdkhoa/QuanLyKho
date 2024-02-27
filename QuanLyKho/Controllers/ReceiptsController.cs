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
using Microsoft.AspNetCore.DataProtection.Repositories;
using QuanLyKho.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using QuanLyKho.Extensions;
using Org.BouncyCastle.Ocsp;
using System.Security.Claims;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Storekeeper,Manager")]
    public class ReceiptsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;
        private readonly IConverter _converter;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public ReceiptsController(AppDbContext context, IReceiptService receiptService, IProductService productService, IConverter converter)
        {
            _context = context;
            _receiptService = receiptService;
            _productService = productService;
            _converter = converter;
        }

        // GET: Receipts
        /// <summary>
        /// Action trả về View danh sách tất cả các kho trong hệ thống
        /// </summary>
        public async Task<IActionResult> Index(string filter = "Show")
        {
            if (_context.Receipts == null)
                return Problem("Entity set 'AppDbContext.Receipts'  is null.");

            var receiptQuery = _context.Receipts.Include(r => r.Staff).Include(r => r.WareHouse).Include(r => r.DestinationWarehouse).AsQueryable();

            if (filter == "Show")
                receiptQuery = receiptQuery.Where(c => c.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                receiptQuery = receiptQuery.Where(c => c.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await receiptQuery.ToListAsync());
        }

        // GET: Receipts/Details/5
        /// <summary>
        /// Action trả về View thông tin chi tiết của phiếu
        /// </summary>
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
        /// <summary>
        /// Action trả về View tạo phiếu Nhập/Xuất kho
        /// </summary>
        public IActionResult Create()
        {
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");

            if (this.User.IsInRole("Admin"))
            {
                ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
            }
            else
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
            }

            return View();
        }

        // POST: Receipts/Create
        /// <summary>
        /// Action trả về View cập nhật thông tin phiếu(GET)
        /// </summary>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,DateCreated,Type,WareHouseId,StaffId")] Receipt receipt)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(receipt);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id", receipt.StaffId);
        //    ViewData["WareHouseId"] = new SelectList(_context.WareHouses.Where(w => w.Status == Status.Show).ToList(), "Id", "Id", receipt.WareHouseId);
        //    ViewData["ProductId"] = new SelectList(_context.Products.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
        //    return View(receipt);
        //}

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
        /// <summary>
        /// Action cập nhật thông tin phiếu(POST)
        /// </summary>
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
        /// <summary>
        /// Action trả về View xác nhận xóa phiếu
        /// </summary>
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
        /// <summary>
        /// Action xóa phiếu
        /// </summary>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Receipts == null)
            {
                return Problem("Entity set 'AppDbContext.Receipts'  is null.");
            }
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt != null && receipt.Status == Status.Show)
            {
                var receiptDetails = await _context.ReceiptDetails.Where(rc => rc.ReceiptId == receipt.Id).ToListAsync();

                if(receipt.Type == ReceiptType.Import)
                {
                    receiptDetails.ForEach(rc =>
                    {
                        var productWarehouse = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == rc.ProductId && pw.WareHouseId == receipt.WareHouseId);
                        if (productWarehouse != null)
                        {
                            productWarehouse.Quantity -= rc.Quantity;
                        }
                    });
                } 
                else if (receipt.Type == ReceiptType.Export)
                {
                    receiptDetails.ForEach(rc =>
                    {
                        var productWarehouse = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == rc.ProductId && pw.WareHouseId == receipt.WareHouseId);
                        if (productWarehouse != null)
                        {
                            productWarehouse.Quantity += rc.Quantity;
                        }
                    });
                }
                else if(receipt.Type == ReceiptType.Transfer)
                {
                    receiptDetails.ForEach(rc =>
                    {
                        var productWarehouse = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == rc.ProductId && pw.WareHouseId == receipt.WareHouseId);
                        if (productWarehouse != null)
                        {
                            productWarehouse.Quantity += rc.Quantity;
                        }

                        var productDestinationWarehouse = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == rc.ProductId && pw.WareHouseId == receipt.DestinationWarehouseId);
                        if (productDestinationWarehouse != null)
                        {
                            productDestinationWarehouse.Quantity -= rc.Quantity;
                        }
                    });
                }

                receipt.Status = Status.Hide;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Phương thức kiểm tra kho đã tồn tại trong hệ thống hay chưa
        /// </summary>
        private bool ReceiptExists(int id)
        {
            return (_context.Receipts?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        /// <summary>
        /// Action tạo phiếu Nhập/Xuất kho
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Receipt receipt, List<string>? ProductIds, List<int>? productQuantitys, List<NewProductModel>? newProducts)
        {
            if (receipt == null || (ProductIds is null && newProducts is null) || (ProductIds?.Count == 0 && newProducts?.Count == 0))
            {
                ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");

                ModelState.AddModelError("", "Please complete all information");


                if (this.User.IsInRole("Admin"))
                {
                    ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
                }
                else
                {
                    string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
                }

                return View(receipt);
            }
            // check số lượng 
            var productsInValid = productQuantitys.Exists(quantity => quantity < 1);
            var newProductInValid = newProducts.Exists(newProduct => newProduct.Quantity < 1);

            if (productsInValid || newProductInValid)
            {
                if (this.User.IsInRole("Admin"))
                {
                    ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
                }
                else
                {
                    string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
                }

                ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
                ModelState.AddModelError("", "The quantity of each product must be greater than 0");
                return View(receipt);
            }


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
                    var products = newProducts.Select(newProduct =>
                    {
                        var product = new Product { Id = newProduct.Id, Name = newProduct.Name, Price = newProduct.Price };
                        product.SetCreatedTime();
                        return product;
                    }).ToList();
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
                receipt.SetCreatedTime();
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

                            if (this.User.IsInRole("Admin"))
                            {
                                ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
                            }
                            else
                            {
                                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                                ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
                            }

                            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                            ViewData["ProductId"] = new MultiSelectList(_context.Products, "Id", "Id", ProductIds);
                            return View("Create", receipt);
                        }
                        // Thực hiện cập nhật ProductWarehouse
                        var productWareHouseExist = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == i.ProductId && pw.WareHouseId == receipt.WareHouseId);
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
                if (this.User.IsInRole("Admin"))
                {
                    ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
                }
                else
                {
                    string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
                }

                ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");

                return View("Create", receipt);
            }
        }

        /// <summary>
        /// Phương thức Render template phiếu Nhập/Xuất/Chuyển kho
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> RenderReceipt(int id)
        {
            if (id == 0)
                return BadRequest();
            var receipt = await _context.Receipts
                                        .Include(r => r.Staff)
                                        .Include(r => r.WareHouse)
                                        .Include(r => r.DestinationWarehouse)
                                        .FirstOrDefaultAsync(r => r.Id == id);
            if (receipt == null)
                return BadRequest();

            receipt.ReceiptDetails = _context.ReceiptDetails.Include(d => d.Product).Where(rd => rd.ReceiptId == id).ToList();

            return View(receipt);
        }

        /// <summary>
        /// Action xuất phiếu
        /// </summary>
        public async Task<IActionResult> ExportReceipt(int id)
        {
            var receipt = _context.Receipts.Include(r => r.ReceiptDetails).FirstOrDefault(r => r.Id == id);
            if (receipt == null)
                return NotFound();

            var stylesPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "lib", "bootstrap", "dist", "css", "bootstrap.css");
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10, Bottom=20, Left=20, Right=20 },
                },
                Objects = {
                            new ObjectSettings()
                            {
                                Page = "https://localhost:7055/Receipts/RenderReceipt/"+id,
                                //HtmlContent = this.generateHtml(receipt), 
                                PagesCount = true,
                                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = stylesPath,  },
                                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                                FooterSettings = { FontName = "Arial", FontSize = 10, Line = true, Center = "SmallHall", Spacing = 5,}
                            },

                        }
            };
            var bytes = _converter.Convert(doc);


            var stream = new MemoryStream(bytes);

            return File(stream, "application/pdf");
        }


        /// <summary>
        /// API Action thống kê phiếu Nhập/Xuất/Chuyển kho => trả về dữ liệu kiểu Json
        /// </summary>
        [HttpPost("/api/receipts/statistic")]
        public async Task<IActionResult> StatisticReceipts(char time, string type, DateTime from, DateTime to, string baseAs, string baseId)
        {
            var receipts = _context.Receipts.AsEnumerable();

            // Check dua tren loai phieu
            if (type == "all")
            {
                // khong lam gi ca
            }
            else if (type == "import")
            {
                receipts = receipts.Where(rc => rc.Type == ReceiptType.Import);
            }
            else if (type == "export")
            {
                receipts = receipts.Where(rc => rc.Type == ReceiptType.Export);
            }
            else if (type == "transfer")
            {
                receipts = receipts.Where(rc => rc.Type == ReceiptType.Transfer);
            }
            else
            {
                return BadRequest();
            }

            // Check dua tren baseAs
            if (string.IsNullOrEmpty(baseAs) || baseAs == "all") // neu de trong thi coi nhu la all
            {
                // lay het tat ca, khong loc, bo trong =)))
            }
            else if (baseAs == "staff") // loc dua tren nhan vien
            {
                receipts = receipts.Where(rc => rc.StaffId == baseId);
            }
            else if (baseAs == "warehouse") // loc dua tren nha kho
            {
                receipts = receipts.Where(rc => rc.WareHouseId == baseId);
            }
            else
                return BadRequest();


            // Check dua tren thoi gian
            if (time == 't')
            {
                receipts = receipts.Where(rc => rc.DateCreated.Date >= from.Date && rc.DateCreated.Date <= to.Date);
            }
            else if (time == 'd')
            {
                receipts = receipts.Where(rc => rc.DateCreated.Date == DateTime.Now.Date);
            }
            else if (time == 'w')
            {
                var startDate = DateTime.Today.AddDays(-7);
                var endDate = DateTime.Today;
                receipts = receipts.Where(rc =>
                {
                    return rc.DateCreated.Date >= startDate && rc.DateCreated.Date <= endDate;
                });
            }
            else if (time == 'm')
            {
                receipts = receipts.Where(rc =>
                {
                    var year = rc.DateCreated.Year;
                    var month = rc.DateCreated.Month;

                    return year == DateTime.Now.Year && month == DateTime.Now.Month;
                });
            }
            else if (time == 'q')
            {
                var quarter = (DateTime.Today.Month - 1) / 3 + 1; // Tính quý hiện tại;
                var year = DateTime.Today.Year; // Năm hiện tại;

                // Tính ngày bắt đầu và kết thúc của quý hiện tại
                var startQuarter = new DateTime(year, 3 * quarter - 2, 1);
                var endQuarter = startQuarter.AddMonths(3).AddDays(-1);

                receipts = receipts.Where(rc =>
                {
                    return rc.DateCreated.Date >= startQuarter && rc.DateCreated.Date <= endQuarter;
                });
            }
            else
            {
                return BadRequest();
            }

            foreach (var receipt in receipts)
            {
                receipt.ReceiptDetails = await _context.ReceiptDetails.Where(rd => rd.ReceiptId == receipt.Id).ToListAsync();
            }
            var kq = receipts.ToList();
            return Json(kq);
        }

        /// <summary>
        /// Phương thức tạo template html của phiếu Nhập/Xuất/Chuyển kho
        /// </summary>
        private string generateHtml(Receipt receipt)
        {
            var receiptName = receipt.Type == ReceiptType.Import ? "Phiếu nhập kho" : "Phiếu xuất kho";
            StringBuilder sb = new StringBuilder();
            sb.Append(@$"
                        <html>
                            <head>
                                <meta charset=""UTF-8"">
                                <title>{receiptName}</title>
                            </head>
                            <body>
                                <div class=""container-fluid pt-5"">
                                    <div id=""print"" class=""row justify-content-center"">
                                        <div class=""col-md-10 col-lg-8"">
                                            <div class=""card "">
                                                <div class=""card-header text-center"">
                                                    <h1 class=""card-title"">{receiptName}</h1>
                                                    <p class=""card-subtitle mb-2 text-muted"">Ngày nhập: {receipt.DateCreated.ToShortDateString()}</p>
                                                </div>
                                                <div class=""card-body px-5"">
                                                    <table class=""table"">
                                                        <thead>
                                                            <tr>
                                                                <th>Mã hàng</th>
                                                                <th>Tên hàng</th>
                                                                <th>Số lượng</th>
                                                                <th>Đơn giá(vnd)</th>
                                                                <th>Thành tiền(vnd)</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>");

            foreach (var detail in receipt.ReceiptDetails)
            {
                var product = _context.Products.Find(detail.ProductId);
                sb.AppendFormat(@$"<tr>
                                        <td>{detail.ProductId}</td>
                                        <td>{product.Name}</td>
                                        <td class=""font-weight-bold"">{detail.Quantity}</td>
                                        <td class=""font-weight-bold"">{Helpers.PriceToVND(detail.Product.Price)}</td>
                                        <td class=""font-weight-bold"">{detail.getAmountToVND()}</td>
                                    </tr>");
            }
            sb.Append(@"</tbody>
                                                    </table>
                                                </div>
                                                <div class=""pb-5 pt-4 d-flex flex-row justify-content-around"">
                                                    <div class=""col-3 pb-5 text-center"">
                                                        <h6 class=""mb-0  font-weight-bold"">Thủ kho</h6>
                                                        <p>(Ký, họ tên)</p>
                                                    </div>
                                                    <div class=""col-3 pb-5 text-center"">
                                                        <h6 class=""mb-0  font-weight-bold"">Thủ kho</h6>
                                                        <p>(Ký, họ tên)</p>
                                                    </div>
                                                    <div class=""col-3 pb-5 text-center"">
                                                        <h6 class=""mb-0  font-weight-bold"">Thủ kho</h6>
                                                        <p>(Ký, họ tên)</p>
                                                    </div><div class=""col-3 pb-5 text-center"">
                                                        <h6 class=""mb-0  font-weight-bold"">Thủ kho</h6>
                                                        <p>(Ký, họ tên)</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </body>
                            </html>");

            return sb.ToString();
        }


        /// <summary>
        /// Action trả về View tạo phiếu Chuyển kho(GET)
        /// </summary>
        public IActionResult CreateTransferReceipt()
        {
            ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");

            if (this.User.IsInRole("Admin"))
            {
                ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
            }
            else
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
            }

            return View();
        }


        /// <summary>
        /// Action tạo phiếu chuyển kho(POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTransferReceipt(Receipt receipt, List<string>? ProductIds, List<int>? productQuantitys)
        {
            if (receipt == null || ProductIds is null)
                return BadRequest();

            List<ReceiptDetail> _receiptDetails = new List<ReceiptDetail>();

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
                receipt.SetCreatedTime();
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
                        ModelState.AddModelError("", $"Product {i.ProductId} in warehouse ({receipt.WareHouseId}) has quantity less than quantity you want transfer");
                        ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id");
                        if (this.User.IsInRole("Admin"))
                        {
                            ViewData["StaffId"] = new SelectList(_context.Staffs.Where(w => w.Status == Status.Show).ToList(), "Id", "Id");
                        }
                        else
                        {
                            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                            ViewData["StaffId"] = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault().Id;
                        }
                        ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
                        //ViewData["ProductId"] = new MultiSelectList(_context.Products, "Id", "Id", ProductIds);
                        return View("CreateTransferReceipt", receipt);
                    }
                    // Thực hiện cập nhật ls ProductWarehouse cho kho lấy hàng trước
                    var productWareHouseExist = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == i.ProductId && pw.WareHouseId == receipt.WareHouseId);
                    if (productWareHouseExist != null) // Kiểm tra xem trong ProductWarehouse đã tồn tại sản phẩm này chưa, nếu đã tồn tại thì cập nhật lại quantity
                    {
                        productWareHouseExist.Quantity -= i.Quantity;

                        // sau đó cập nhật sl productWarehouse cho kho đích đến
                        var destinationProductWarehouseExist = _context.ProductWareHouses.FirstOrDefault(pw => pw.ProductId == i.ProductId && pw.WareHouseId == receipt.DestinationWarehouseId);
                        if (destinationProductWarehouseExist != null)
                        {
                            destinationProductWarehouseExist.Quantity += i.Quantity;
                        }
                        else // kho này chưa có hàng này (thêm mới productWarehouse cho kho này)
                        {
                            var newProductWarehouse = new ProductWareHouse
                            {
                                ProductId = i.ProductId,
                                WareHouseId = receipt.DestinationWarehouseId,
                                Quantity = i.Quantity,
                                Status = Status.Show,
                            };
                            newProductWarehouse.SetCreatedTime();
                            await _context.ProductWareHouses.AddAsync(newProductWarehouse);
                        }
                    }
                    else // có lỗi gì đó xảy ra
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Error");
                        ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Id");
                        ViewData["WareHouseId"] = new SelectList(_context.WareHouses, "Id", "Id");
                        ViewData["ProductId"] = new MultiSelectList(_context.Products, "Id", "Id", ProductIds);
                        return View("CreateTransferReceipt", receipt);
                    }
                }

                await _context.ReceiptDetails.AddRangeAsync(_receiptDetails);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
