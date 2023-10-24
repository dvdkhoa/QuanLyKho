using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

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


        public IActionResult Details(string id)
        {
            var customer = _context.Customer.Find(id);
            if(customer == null)
                return NotFound();

            return View(customer);
        }


        public IActionResult Edit(string id)
        {
            var customer = _context.Customer.Find(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

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


        public async Task<IActionResult> Display(string id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer is null)
                return NotFound();

            customer.Status = customer.Status.ChangeStatus();
            customer.UpdateTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }
    }
}
