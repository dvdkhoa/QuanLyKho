using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Controllers
{
    public class WareHousesController : Controller
    {
        private readonly AppDbContext _context;

        public WareHousesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: WareHouses
        public async Task<IActionResult> Index()
        {
              return _context.WareHouses != null ? 
                          View(await _context.WareHouses.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.WareHouses'  is null.");
        }

        // GET: WareHouses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.WareHouses == null)
            {
                return NotFound();
            }

            var wareHouse = await _context.WareHouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wareHouse == null)
            {
                return NotFound();
            }

            return View(wareHouse);
        }

        // GET: WareHouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WareHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Address")] WareHouse wareHouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wareHouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wareHouse);
        }

        // GET: WareHouses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,PhoneNumber,Address")] WareHouse wareHouse)
        {
            if (id != wareHouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: WareHouses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.WareHouses == null)
            {
                return NotFound();
            }

            var wareHouse = await _context.WareHouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wareHouse == null)
            {
                return NotFound();
            }

            return View(wareHouse);
        }

        // POST: WareHouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.WareHouses == null)
            {
                return Problem("Entity set 'AppDbContext.WareHouses'  is null.");
            }
            var wareHouse = await _context.WareHouses.FindAsync(id);
            if (wareHouse != null)
            {
                _context.WareHouses.Remove(wareHouse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WareHouseExists(string id)
        {
          return (_context.WareHouses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
