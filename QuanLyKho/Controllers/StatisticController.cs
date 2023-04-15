using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.DTO;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace QuanLyKho.Controllers
{
    public class StatisticController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStatisticService _statisticService;
        private readonly IProductService _productService;

        public StatisticController(IStatisticService statisticService, AppDbContext context, IProductService productService)
        {
            _statisticService = statisticService;
            _context = context;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductStatistic()
        {
            ViewData["warehouses"] = _context.WareHouses.ToList();
            return View();
        }

        [HttpPost("/api/receipts/productStatistic")]
        public IActionResult ProductStatisticPost(string time, DateTime dateFrom, DateTime dateTo, string type, string warehouseId)
        {
            if (string.IsNullOrEmpty(time) || string.IsNullOrEmpty(type))
            {
                return BadRequest();
            }

            var products = _productService.GetProductByWarehouseId(warehouseId);

            var receiptDetail_receipts = (from r in _context.Receipts
                                          join rd in _context.ReceiptDetails on r.Id equals rd.ReceiptId
                                          select new { r, rd }).AsQueryable();

            List<ProductStatisticInfoModel> product_details = null;


            if (type == "recently_import")
            {
                var import_temp = (from a in receiptDetail_receipts
                                   where a.r.Type == ReceiptType.Import
                                   select new { a.r, a.rd }).ToList();
                // Check dựa trên thời gian
                if (time == "d")
                {
                    var temp = (from a in import_temp
                                where a.r.WareHouseId == warehouseId && a.r.DateCreated.Date == DateTime.Now.Date // dieu kien o day duoc truyen vao
                                select new { a.r, a.rd }).ToList();
                    var details = (from a in temp
                                   join p in products on a.rd.ProductId equals p.Id
                                   select new { p, a.r, a.rd }).ToList();

                    product_details = details.Select(i =>
                    {
                        return new ProductStatisticInfoModel
                        {
                            Id = i.p.Id,
                            DateImported = i.r.DateCreated.Date,
                            Imported = i.rd.Quantity,
                            Price = i.p.Price,
                            Name = i.p.Name,
                            Status = i.p.Status,
                            Unit = i.p.Unit,
                            InputMoney = i.rd.getAmountToVND(),
                            InventoryNumber = _productService.GetProductQuantity(i.p.Id)
                        };
                    }).ToList();
                }
                else if (time == "w")
                {
                    var startDate = DateTime.Today.AddDays(-7);
                    var endDate = DateTime.Today;

                    var temp = (from a in import_temp
                                where a.r.WareHouseId == warehouseId && a.r.DateCreated.Date <= startDate && a.r.DateCreated.Date >= endDate
                                select new { a.r, a.rd }).ToList();
                    var details = (from a in temp
                                   join p in products on a.rd.ProductId equals p.Id
                                   select new { p, a.r, a.rd }).ToList();

                    product_details = details.Select(i =>
                    {
                        return new ProductStatisticInfoModel
                        {
                            Id = i.p.Id,
                            DateImported = i.r.DateCreated.Date,
                            Imported = i.rd.Quantity,
                            Price = i.p.Price,
                            Name = i.p.Name,
                            Status = i.p.Status,
                            Unit = i.p.Unit,
                            InputMoney = i.rd.getAmountToVND(),
                            InventoryNumber = _productService.GetProductQuantity(i.p.Id)
                        };
                    }).ToList();
                }
                else if (time == "m")
                {
                    var year = DateTime.Now.Year;
                    var month = DateTime.Now.Month;

                    var temp = (from a in import_temp
                                where a.r.WareHouseId == warehouseId && a.r.DateCreated.Year == year && a.r.DateCreated.Month == month
                                select new { a.r, a.rd }).ToList();
                    var details = (from a in temp
                                   join p in products on a.rd.ProductId equals p.Id
                                   select new { p, a.r, a.rd }).ToList();

                    product_details = details.Select(i =>
                    {
                        return new ProductStatisticInfoModel
                        {
                            Id = i.p.Id,
                            DateImported = i.r.DateCreated.Date,
                            Imported = i.rd.Quantity,
                            Price = i.p.Price,
                            Name = i.p.Name,
                            Status = i.p.Status,
                            Unit = i.p.Unit,
                            InputMoney = i.rd.getAmountToVND(),
                            InventoryNumber = _productService.GetProductQuantity(i.p.Id)
                        };
                    }).ToList();
                }
                else if (time == "q")
                {
                    var quarter = (DateTime.Today.Month - 1) / 3 + 1; // Tính quý hiện tại;
                    var year = DateTime.Today.Year; // Năm hiện tại;

                    // Tính ngày bắt đầu và kết thúc của quý hiện tại
                    var startQuarter = new DateTime(year, 3 * quarter - 2, 1);
                    var endQuarter = startQuarter.AddMonths(3).AddDays(-1);

                    var temp = (from a in import_temp
                                where a.r.WareHouseId == warehouseId && a.r.DateCreated.Date >= startQuarter && a.r.DateCreated.Date <= endQuarter
                                select new { a.r, a.rd }).ToList();
                    var details = (from a in temp
                                   join p in products on a.rd.ProductId equals p.Id
                                   select new { p, a.r, a.rd }).ToList();

                    product_details = details.Select(i =>
                    {
                        return new ProductStatisticInfoModel
                        {
                            Id = i.p.Id,
                            DateImported = i.r.DateCreated.Date,
                            Imported = i.rd.Quantity,
                            Price = i.p.Price,
                            Name = i.p.Name,
                            Status = i.p.Status,
                            Unit = i.p.Unit,
                            InputMoney = i.rd.getAmountToVND(),
                            InventoryNumber = _productService.GetProductQuantity(i.p.Id)
                        };
                    }).ToList();
                }
                else if (time == "t")
                {
                    var temp = (from a in import_temp
                                where a.r.WareHouseId == warehouseId && a.r.DateCreated.Date >= dateFrom.Date && a.r.DateCreated.Date <= dateTo.Date
                                select new { a.r, a.rd }).ToList();
                    var details = (from a in temp
                                   join p in products on a.rd.ProductId equals p.Id
                                   select new { p, a.r, a.rd }).ToList();

                    product_details = details.Select(i =>
                    {
                        return new ProductStatisticInfoModel
                        {
                            Id = i.p.Id,
                            DateImported = i.r.DateCreated.Date,
                            Imported = i.rd.Quantity,
                            Price = i.p.Price,
                            Name = i.p.Name,
                            Status = i.p.Status,
                            Unit = i.p.Unit,
                            InputMoney = i.rd.getAmountToVND(),
                            InventoryNumber = _productService.GetProductQuantity(i.p.Id)
                        };
                    }).ToList();
                }
                else
                    return BadRequest();

                return Ok(product_details);
            }
            else if (type == "out_of_stock")
            {
                var list_product_out_of_stock = _context.ProductWareHouses.Include(pw => pw.Product).Where(pw => pw.WareHouseId == warehouseId && pw.Quantity == 0).ToList();

                var productStatistics =  list_product_out_of_stock.Select(p =>
                {
                    var importedDate = _context.ReceiptDetails.Include(rd=>rd.Receipt).OrderBy(rd=>rd.Id).LastOrDefault(rd=>rd.ProductId == p.ProductId && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import).Receipt.DateCreated;
                    return new ProductStatisticInfoModel
                    {
                        Id = p.ProductId,
                        Price = p.Product.Price,
                        Name = p.Product.Name,
                        Status = p.Product.Status,
                        Unit = p.Product.Unit,
                        InventoryNumber = p.Quantity,
                        DateImported = importedDate
                    };
                }).ToList();

                return Ok(productStatistics);
            }
            else if (type == "inventory")
            {

            }
            else
                return BadRequest();


            return BadRequest();
        }


        public async Task<IActionResult> ProductInStock()
        {
            var productInStock = await _statisticService.GetProductInStocks();

            return View(productInStock);
        }


    }
}
