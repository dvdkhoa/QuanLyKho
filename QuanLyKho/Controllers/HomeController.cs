using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;
using System.Diagnostics;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Action trả về View Dashboard
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var thisMonth = DateTime.Today.Month;
            var thisYear = DateTime.Today.Year;

            ViewBag.newOrders = await _context.Orders.CountAsync(o => o.CreatedTime.Month == thisMonth && o.CreatedTime.Year == thisYear);
            ViewBag.successOrders = await _context.Orders.CountAsync(o => o.CreatedTime.Month == thisMonth && o.CreatedTime.Year == thisYear && o.ShipStatus == ShipStatus.Success);
            ViewBag.failOrders = await _context.Orders.CountAsync(o => o.CreatedTime.Month == thisMonth && o.CreatedTime.Year == thisYear && o.ShipStatus == ShipStatus.Canceled);
            ViewBag.shippingOrders = await _context.Orders.CountAsync(o => o.CreatedTime.Month == thisMonth && o.CreatedTime.Year == thisYear && o.ShipStatus == ShipStatus.BeingShipped);

            ViewBag.newCustomer = await _context.Customer.CountAsync(cus => cus.CreatedTime.Month == thisMonth && cus.CreatedTime.Year == thisYear);
            ViewBag.promotions = await _context.Promotions.CountAsync(pro => pro.StartDate <= DateTime.Now && DateTime.Now < pro.EndDate);
            ViewBag.news = await _context.News.CountAsync(n => n.CreatedTime.Month == thisMonth && n.CreatedTime.Year == thisYear);
            ViewBag.newProducts = await _context.Products.CountAsync(p => p.CreatedTime.Month == thisMonth && p.CreatedTime.Year == thisYear);

            ViewBag.totalRevenue = await this.TotalRevenueInYear();

            ViewBag.totalRevenueByMonth = await TotalRevenueByMonth();

            ViewBag.orderInMonth = await getOrderInMonth(thisMonth, thisYear);

            return View();
        }

        /// <summary>
        /// Phương thức tính tổng doanh thu trong năm hiện tại
        /// </summary>
        private async Task<double> TotalRevenueInYear()
        {
            var thisYear = DateTime.Today.Year;

            double totalRevenue = await _context.Orders.Where(o => o.ShipStatus == ShipStatus.Success && o.CreatedTime.Year == thisYear).SumAsync(o => o.Total);

            return totalRevenue;
        }

        /// <summary>
        /// Phương thức lấy tất cả các đơn hàng trong năm => thống kê theo tháng
        /// </summary>
        private async Task<List<Order>> TotalRevenueByMonth()
        {
            var thisYear = DateTime.Today.Year;

            var orders = await (from o in _context.Orders
                                where o.ShipStatus == ShipStatus.Success && o.CreatedTime.Year == thisYear
                                select o).ToListAsync();

            orders.ForEach(o =>
            {
                o.OrderDetails = _context.OrderDetails.Where(od => od.OrderId == o.Id).ToList();
            });

            return orders;
        }


        /// <summary>
        /// Phương thức lấy tất cả các đơn hàng trong tháng
        /// </summary>
        private async Task<List<Order>> getOrderInMonth(int month, int year)
        {
            var orders = await _context.Orders.Where(o => o.CreatedTime.Month == month && o.CreatedTime.Year == year && o.ShipStatus == ShipStatus.Success).ToListAsync();

            return orders;
        }

        /// <summary>
        /// Action trả về View Privacy
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Action trả về View báo lỗi
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}