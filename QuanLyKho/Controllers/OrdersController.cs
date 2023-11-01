using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using System.Security.Claims;
using System.Text.Json;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager,Sales staff")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{

        //    if (_context.Orders == null)
        //        return Problem("Entity set 'AppDbContext.Orders'  is null.");

        //    return View(await _context.Orders.ToListAsync());
        //}

        public async Task<IActionResult> Index(string filter = "All")
        {
            var ordersQuery = _context.Orders.AsQueryable();

            if (filter == "Success")
                ordersQuery = ordersQuery.Where(product => product.ShipStatus == ShipStatus.Success).AsQueryable();
            else if (filter == "BeingShipped")
                ordersQuery = ordersQuery.Where(product => product.ShipStatus == ShipStatus.BeingShipped).AsQueryable();
            else if (filter == "NotApproved")
                ordersQuery = ordersQuery.Where(product => product.ShipStatus == ShipStatus.NotApproved).AsQueryable();
            else if (filter == "Canceled")
                ordersQuery = ordersQuery.Where(product => product.ShipStatus == ShipStatus.Canceled).AsQueryable();

            ViewBag.filter = filter;
            return View(await ordersQuery.ToListAsync());
        }


        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders.Include(o => o.Store)
                                              .Include(o => o.Staff)
                                              .FirstOrDefaultAsync(o => o.Id == id);
            if (order is null)
                return NotFound();

            order.OrderDetails = await _context.OrderDetails.Where(od => od.OrderId == id).ToListAsync();

            ViewData["warehouses"] = _context.ProductWareHouses.Include(pw => pw.WareHouse).ToList();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int orderId, string orderDetailIdsJson, List<int> productWarehouseIds, bool? isComplete)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order is null)
                return NotFound();

            if (isComplete != null)
            {
                if (isComplete.Value)
                {
                    order.ShipStatus = ShipStatus.Success;
                    order.SetUpdatedTime();
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", orderId);
                }
            }

            if (order.ShipStatus != ShipStatus.NotApproved)
            {
                return RedirectToAction("Details", order);
            }

            order.Staff = await _context.Staffs.FindAsync(order.StaffId);
            order.Customer = await _context.Customer.FindAsync(order.CustomerId);
            order.Store = await _context.WareHouses.FindAsync(order.StoreId);


            var orderDetailIds = JsonSerializer.Deserialize<List<int>>(orderDetailIdsJson);

            if (orderDetailIds!.Count != productWarehouseIds.Count)
            {
                return BadRequest();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var isNull = productWarehouseIds.Any(id => id == 0);

                    if (isNull)
                    {
                        ModelState.AddModelError("", "Some order details have not been approved");

                        order.OrderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();

                        ViewData["warehouses"] = _context.ProductWareHouses.Include(pw => pw.WareHouse).ToList();

                        return View(order);
                    }
                    // duyệt đơn ở đây
                    for (int i = 0; i < orderDetailIds.Count; i++)
                    {
                        var orderDetail = await _context.OrderDetails.FindAsync(orderDetailIds[i]);
                        if (orderDetail != null)
                        {
                            orderDetail.ProductWarehouseId = productWarehouseIds[i];
                            var productWarehouse = await _context.ProductWareHouses.Include(pw => pw.WareHouse).Include(pw => pw.Product).FirstOrDefaultAsync(pw => pw.Id == productWarehouseIds[i]);
                            if (productWarehouse!.Quantity < orderDetail.Quantity) // số lượng còn lại bị âm => sai
                            {
                                ModelState.AddModelError("", $"Warehouse '{productWarehouse.WareHouse.Name}' does not have enough product '{productWarehouse.Product.Name}' for this order");
                            }
                            else
                                productWarehouse!.Quantity -= orderDetail.Quantity;
                        }
                    }
                    if (ModelState.ErrorCount > 0)
                    {
                        order.OrderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();

                        ViewData["warehouses"] = _context.ProductWareHouses.Include(pw => pw.WareHouse).ToList();

                        return View(order);
                    }

                    order.ShipStatus = ShipStatus.BeingShipped;

                    if (!this.User.IsInRole("Admin"))
                    {
                        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        string staffId = _context.Staffs.Where(s => s.UserId == userId).FirstOrDefault()!.Id;

                        order.StaffId = staffId;
                    }

                    order.SetUpdatedTime();

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Something went wrong: {ex.Message}");
                    await transaction.RollbackAsync();
                }
            }
            order.OrderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();

            ViewData["warehouses"] = _context.ProductWareHouses.Include(pw => pw.WareHouse).ToList();

            return View(order);
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order is null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order orderSubmit, ShipStatus oldShipStatus)
        {
            ModelState.Remove("StaffId");
            ModelState.Remove("CustomerId");

            if (ModelState.IsValid)
            {

                if (orderSubmit.ShipStatus == ShipStatus.Success)
                {
                    orderSubmit.PaymentStatus = PaymentStatus.Paid;
                }
                else if (orderSubmit.ShipStatus == ShipStatus.Canceled)
                {
                    if (oldShipStatus == ShipStatus.NotApproved || oldShipStatus == ShipStatus.Canceled)
                    {
                        // Không cần backup dữ liệu

                    }
                    else
                    {
                        // Backup dữ liệu ở đây
                        var orderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderSubmit.Id).ToListAsync();

                        foreach (var detail in orderDetails)
                        {
                            var productWarehouse = await _context.ProductWareHouses.FindAsync(detail.ProductWarehouseId);
                            productWarehouse!.Quantity += detail.Quantity;
                        }
                    }
                }


                orderSubmit.SetUpdatedTime();


                _context.Update(orderSubmit);

                var kq = await _context.SaveChangesAsync();
                if (kq > 0)
                {
                    orderSubmit.OrderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderSubmit.Id).ToListAsync();

                    ViewData["warehouses"] = _context.ProductWareHouses.Include(pw => pw.WareHouse).ToList();

                    return RedirectToAction("Details", orderSubmit);
                }

                return View(orderSubmit);
            }
            return View(orderSubmit);
        }
    }
}
