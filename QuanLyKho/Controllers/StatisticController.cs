using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin,Manager,Storekeeper")]
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

            var products = new List<Product>();
            if (!string.IsNullOrEmpty(warehouseId) && warehouseId != "all")
            {
                products = _productService.GetProductByWarehouseId(warehouseId);

            }
            else
            {
                products = _context.Products.ToList();
            }

            var receiptDetail_receipts = (from r in _context.Receipts
                                          join rd in _context.ReceiptDetails on r.Id equals rd.ReceiptId
                                          select new { r, rd }).AsQueryable();

            List<ProductStatisticInfoModel> product_details = null;


            if (type == "recently_import")
            {
                if (!string.IsNullOrEmpty(warehouseId) && warehouseId != "all")
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
                else
                {
                    var import_temp = (from a in receiptDetail_receipts
                                       where a.r.Type == ReceiptType.Import
                                       select new { a.r, a.rd }).ToList();
                    // Check dựa trên thời gian
                    if (time == "d")
                    {
                        var temp = (from a in import_temp
                                    where a.r.DateCreated.Date == DateTime.Now.Date // dieu kien o day duoc truyen vao
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
                                InventoryNumber = _productService.GetProductQuantity(i.p.Id),
                                WarehouseId = i.r.WareHouseId
                            };
                        }).ToList();
                    }
                    else if (time == "w")
                    {
                        var startDate = DateTime.Today.AddDays(-7);
                        var endDate = DateTime.Today;

                        var temp = (from a in import_temp
                                    where a.r.DateCreated.Date <= startDate && a.r.DateCreated.Date >= endDate
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
                                InventoryNumber = _productService.GetProductQuantity(i.p.Id),
                                WarehouseId = i.r.WareHouseId
                            };
                        }).ToList();
                    }
                    else if (time == "m")
                    {
                        var year = DateTime.Now.Year;
                        var month = DateTime.Now.Month;

                        var temp = (from a in import_temp
                                    where a.r.DateCreated.Year == year && a.r.DateCreated.Month == month
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
                                InventoryNumber = _productService.GetProductQuantity(i.p.Id),
                                WarehouseId = i.r.WareHouseId
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
                                    where a.r.DateCreated.Date >= startQuarter && a.r.DateCreated.Date <= endQuarter
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
                                InventoryNumber = _productService.GetProductQuantity(i.p.Id),
                                WarehouseId = i.r.WareHouseId
                            };
                        }).ToList();
                    }
                    else if (time == "t")
                    {
                        var temp = (from a in import_temp
                                    where a.r.DateCreated.Date >= dateFrom.Date && a.r.DateCreated.Date <= dateTo.Date
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
                                InventoryNumber = _productService.GetProductQuantity(i.p.Id),
                                WarehouseId = i.r.WareHouseId
                            };
                        }).ToList();
                    }
                    else
                        return BadRequest();

                    return Ok(product_details);
                }
            }
            else if (type == "out_of_stock")
            {


                if (!string.IsNullOrEmpty(warehouseId) && warehouseId != "all") // Nếu warehouseId có giá trị cụ thể thì lọc dựa trên warehouseId và join bình thường
                {
                    var out_of_stock_products = (from p in _context.Products
                                                 join pw in _context.ProductWareHouses on p.Id equals pw.ProductId
                                                 where pw.Quantity == 0 && pw.WareHouseId == warehouseId
                                                 select new { p, pw }).ToList();


                    var productStatistics = out_of_stock_products.Select(a =>
                    {
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == a.p.Id && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;
                        return new ProductStatisticInfoModel
                        {
                            Id = a.p.Id,
                            Price = a.p.Price,
                            Name = a.p.Name,
                            Status = a.p.Status,
                            Unit = a.p.Unit,
                            InventoryNumber = a.pw?.Quantity ?? 0,
                            DateImported = importedDate
                        };
                    }).ToList();

                    return Ok(productStatistics);
                }
                else // Ngược lại, nếu warehouseId null hoặc = 'all' thì left join
                {
                    var out_of_stock_products = (from p in _context.Products
                                                 join pw in _context.ProductWareHouses.Where(pw => pw.Quantity == 0) on p.Id equals pw.ProductId into a
                                                 from b in a.DefaultIfEmpty()
                                                 select new { p, pw = b }).ToList();


                    var productStatistics = out_of_stock_products.Select(a =>
                    {
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == a.p.Id && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;
                        return new ProductStatisticInfoModel
                        {
                            Id = a.p.Id,
                            Price = a.p.Price,
                            Name = a.p.Name,
                            Status = a.p.Status,
                            Unit = a.p.Unit,
                            InventoryNumber = a.pw?.Quantity ?? 0,
                            DateImported = importedDate
                        };
                    }).ToList();

                    return Ok(productStatistics);
                }
            }
            else if (type == "inventory")
            {
                var product_temp = new List<Product>();
                foreach (var p in products)
                {
                    var quantity = _productService.GetProductQuantity(p.Id);

                    if (quantity != 0)
                        product_temp.Add(p);
                }

                var productStatistics = new List<ProductStatisticInfoModel>();

                if (time == "d")
                {
                    product_temp.ForEach(p =>
                    {
                        var quantity = _productService.GetProductQuantity(p.Id);
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == p.Id && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;

                        var date = DateTime.Now.AddDays(-1);
                        if (importedDate?.Date == date.Date)
                        {
                            productStatistics.Add(new ProductStatisticInfoModel
                            {
                                Id = p.Id,
                                Price = p.Price,
                                Name = p.Name,
                                Status = p.Status,
                                Unit = p.Unit,
                                InventoryNumber = quantity,
                                DateImported = importedDate,
                                WarehouseId = warehouseId
                            });
                        }
                    });
                }
                else if (time == "w")
                {
                    product_temp.ForEach(p =>
                    {
                        var quantity = _productService.GetProductQuantity(p.Id);
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == p.Id && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;

                        var startDay = DateTime.Now.AddDays(-14);
                        var endDay = DateTime.Today.AddDays(-7);
                        if (importedDate?.Date >= startDay && importedDate <= endDay)
                        {
                            productStatistics.Add(new ProductStatisticInfoModel
                            {
                                Id = p.Id,
                                Price = p.Price,
                                Name = p.Name,
                                Status = p.Status,
                                Unit = p.Unit,
                                InventoryNumber = quantity,
                                DateImported = importedDate,
                                WarehouseId = warehouseId
                            });
                        }
                    });
                }
                else if (time == "m")
                {
                    product_temp.ForEach(p =>
                    {
                        var quantity = _productService.GetProductQuantity(p.Id);
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == p.Id && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;

                        var month = DateTime.Today.Month;
                        var year = DateTime.Today.Year;
                        if (importedDate?.Month == month && importedDate?.Year == year)
                        {
                            productStatistics.Add(new ProductStatisticInfoModel
                            {
                                Id = p.Id,
                                Price = p.Price,
                                Name = p.Name,
                                Status = p.Status,
                                Unit = p.Unit,
                                InventoryNumber = quantity,
                                DateImported = importedDate,
                                WarehouseId = warehouseId
                            });
                        }
                    });
                }
                else if (time == "q")
                {
                    product_temp.ForEach(p =>
                    {
                        var quantity = _productService.GetProductQuantity(p.Id);
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == p.Id && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;

                        var quarter = (DateTime.Today.Month - 1) / 3 + 1; // Tính quý hiện tại;
                        var year = DateTime.Today.Year; // Năm hiện tại;

                        // Tính ngày bắt đầu và kết thúc của quý hiện tại
                        var startQuarter = new DateTime(year, 3 * quarter - 2, 1);
                        var endQuarter = startQuarter.AddMonths(3).AddDays(-1);

                        if (importedDate?.Date >= startQuarter && importedDate?.Date <= endQuarter)
                        {
                            productStatistics.Add(new ProductStatisticInfoModel
                            {
                                Id = p.Id,
                                Price = p.Price,
                                Name = p.Name,
                                Status = p.Status,
                                Unit = p.Unit,
                                InventoryNumber = quantity,
                                DateImported = importedDate,
                                WarehouseId = warehouseId
                            });
                        }
                    });
                }
                else if (time == "t")
                {
                    product_temp.ForEach(p =>
                    {
                        var quantity = _productService.GetProductQuantity(p.Id);
                        var importedDate = _context.ReceiptDetails.Include(rd => rd.Receipt).OrderBy(rd => rd.Id).LastOrDefault(rd => rd.ProductId == p.Id && rd.Receipt.WareHouseId == warehouseId && rd.Receipt.Type == ReceiptType.Import)?.Receipt.DateCreated;

                        if (importedDate?.Date >= dateFrom && importedDate?.Date <= dateTo)
                        {
                            productStatistics.Add(new ProductStatisticInfoModel
                            {
                                Id = p.Id,
                                Price = p.Price,
                                Name = p.Name,
                                Status = p.Status,
                                Unit = p.Unit,
                                InventoryNumber = quantity,
                                DateImported = importedDate,
                                WarehouseId = warehouseId
                            });
                        }
                    });
                }
                else
                    return BadRequest();

                return Ok(productStatistics);
            }
            else
                return BadRequest();
        }


        public async Task<IActionResult> ProductInStock()
        {
            var productInStock = await _statisticService.GetProductInStocks();

            return View(productInStock);
        }


    }
}
