using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager,Sales staff")]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Action trả về View danh sách tất cả khách hàng có trong hệ thống
        /// </summary>
        public async Task<IActionResult> Index(string filter = "All")
        {
            if (_context.Customer == null)
                return Problem("Entity set 'AppDbContext.Customers' is null.");

            var customerQuery = _context.Customer.AsQueryable();

            if (filter == "Show")
                customerQuery = customerQuery.Where(c => c.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                customerQuery = customerQuery.Where(c => c.Status == Status.Hide).AsQueryable();

            ViewData["filter"] = filter;

            return View(await customerQuery.ToListAsync());
        }


        /// <summary>
        /// Action trả về View thông tin chi tiết của khách hàng
        /// </summary>
        public IActionResult Details(string id)
        {
            var customer = _context.Customer.Find(id);
            if (customer == null)
                return NotFound();

            customer.User = _context.Users.Find(customer.UserId);
            customer.Orders = _context.Orders.Where(o => o.CustomerId == id).Include(o => o.Store).ToList();

            return View(customer);
        }


        /// <summary>
        /// Action trả về View cập nhật thông tin của khách hàng(GET)
        /// </summary>
        public IActionResult Edit(string id)
        {
            var customer = _context.Customer.Find(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        /// <summary>
        /// Action cập nhật thông tin khách hàng(POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Update(customer);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", customer);
            }
            return View(customer);
        }


        /// <summary>
        /// Action Ẩn/Hiện khách hàng
        /// </summary>
        public async Task<IActionResult> Display(string id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer is null)
                return NotFound();

            customer.Status = customer.Status.ChangeStatus();
            customer.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }

        /// <summary>
        /// Action xóa khách hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var customer = await _context.Customer.FindAsync(id);
                if (customer is null)
                    return NotFound();

                _context.Customer.Remove(customer);

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

        [AllowAnonymous]
        [HttpPost("/api/customers/changeImage")]
        public async Task<IActionResult> ChangeImage(string id, IFormFile imgFile)
        {
            if (imgFile is null || string.IsNullOrEmpty(id))
                return BadRequest();

            var customer = await _context.Customer.FindAsync(id);

            if (customer is null)
                return NotFound();

            var imgPath = await CloudinaryHelper.UploadFileToCloudinary(imgFile, "Customers");
            if (imgPath != null)
            {
                customer.Image = imgPath;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
