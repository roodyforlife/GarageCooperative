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
    public class TypeGaragesController : Controller
    {
        private readonly DataBaseContext _context;

        public TypeGaragesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: TypeGarages
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.TypeGarages.Include(t => t.Garage).Include(t => t.Type);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: TypeGarages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeGarage = await _context.TypeGarages
                .Include(t => t.Garage)
                .Include(t => t.Type)
                .FirstOrDefaultAsync(m => m.TypeGarageId == id);
            if (typeGarage == null)
            {
                return NotFound();
            }

            return View(typeGarage);
        }

        // GET: TypeGarages/Create
        public IActionResult Create()
        {
            ViewData["GarageId"] = new SelectList(_context.Garages, "GarageId", "GarageId");
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId");
            return View();
        }

        // POST: TypeGarages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeGarageId,GarageId,TypeId")] TypeGarage typeGarage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeGarage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GarageId"] = new SelectList(_context.Garages, "GarageId", "GarageId", typeGarage.GarageId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", typeGarage.TypeId);
            return View(typeGarage);
        }

        // GET: TypeGarages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeGarage = await _context.TypeGarages.FindAsync(id);
            if (typeGarage == null)
            {
                return NotFound();
            }
            ViewData["GarageId"] = new SelectList(_context.Garages, "GarageId", "GarageId", typeGarage.GarageId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", typeGarage.TypeId);
            return View(typeGarage);
        }

        // POST: TypeGarages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeGarageId,GarageId,TypeId")] TypeGarage typeGarage)
        {
            if (id != typeGarage.TypeGarageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeGarage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeGarageExists(typeGarage.TypeGarageId))
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
            ViewData["GarageId"] = new SelectList(_context.Garages, "GarageId", "GarageId", typeGarage.GarageId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", typeGarage.TypeId);
            return View(typeGarage);
        }

        // GET: TypeGarages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeGarage = await _context.TypeGarages
                .Include(t => t.Garage)
                .Include(t => t.Type)
                .FirstOrDefaultAsync(m => m.TypeGarageId == id);
            if (typeGarage == null)
            {
                return NotFound();
            }

            return View(typeGarage);
        }

        // POST: TypeGarages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeGarage = await _context.TypeGarages.FindAsync(id);
            _context.TypeGarages.Remove(typeGarage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeGarageExists(int id)
        {
            return _context.TypeGarages.Any(e => e.TypeGarageId == id);
        }
    }
}
