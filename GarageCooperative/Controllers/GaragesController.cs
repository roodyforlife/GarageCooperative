using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageCooperative.DataBase;
using GarageCooperative.Models;

namespace GarageCooperative.Controllers
{
    public class GaragesController : Controller
    {
        private readonly DataBaseContext _context;

        public GaragesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Garages
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Garages.Include(g => g.Row).Include(g => g.Type);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Garages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await _context.Garages
                .Include(g => g.Row)
                .Include(g => g.Type)
                .Include(x => x.Fees)
                .Include(x => x.Memberships)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(m => m.GarageId == id);
            if (garage == null)
            {
                return NotFound();
            }

            return View(garage);
        }

        // GET: Garages/Create
        public IActionResult Create()
        {
            ViewData["RowId"] = new SelectList(_context.Rows, "RowId", "RowNumber");
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "Name");
            return View();
        }

        // POST: Garages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GarageId,Number,GarageSpace,CarsCapacity,RowId,TypeId")] Garage garage)
        {
            if (await _context.Garages.Include(x => x.Row).FirstOrDefaultAsync(x => x.Number == garage.Number && x.Row.RowId == garage.RowId) is not null)
            {
                ModelState.AddModelError("Number", "Garage already registered");
            }

            if (ModelState.IsValid)
            {
                _context.Add(garage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RowId"] = new SelectList(_context.Rows, "RowId", "RowNumber", garage.RowId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "Name", garage.TypeId);
            return View(garage);
        }

        // GET: Garages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await _context.Garages.FindAsync(id);
            if (garage == null)
            {
                return NotFound();
            }
            ViewData["RowId"] = new SelectList(_context.Rows, "RowId", "RowNumber", garage.RowId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "Name", garage.TypeId);
            return View(garage);
        }

        // POST: Garages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GarageId,Number,GarageSpace,CarsCapacity,RowId,TypeId")] Garage garage)
        {
            if (id != garage.GarageId)
            {
                return NotFound();
            }

            if (await _context.Garages.Include(x => x.Row).FirstOrDefaultAsync(x => x.Number == garage.Number
                && x.Row.RowId == garage.RowId && x.GarageId != garage.GarageId) is not null)
            {
                ModelState.AddModelError("Number", "Garage already registered");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(garage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarageExists(garage.GarageId))
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
            ViewData["RowId"] = new SelectList(_context.Rows, "RowId", "RowNumber", garage.RowId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "Name", garage.TypeId);
            return View(garage);
        }

        // GET: Garages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await _context.Garages
                .Include(g => g.Row)
                .Include(g => g.Type)
                .FirstOrDefaultAsync(m => m.GarageId == id);
            if (garage == null)
            {
                return NotFound();
            }

            return View(garage);
        }

        // POST: Garages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garage = await _context.Garages.FindAsync(id);
            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GarageExists(int id)
        {
            return _context.Garages.Any(e => e.GarageId == id);
        }
    }
}
