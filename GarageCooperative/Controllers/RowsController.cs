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
    public class RowsController : Controller
    {
        private readonly DataBaseContext _context;

        public RowsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Rows
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Rows.Include(r => r.Cooperative);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Rows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var row = await _context.Rows
                .Include(r => r.Cooperative)
                .FirstOrDefaultAsync(m => m.RowId == id);
            if (row == null)
            {
                return NotFound();
            }

            return View(row);
        }

        // GET: Rows/Create
        public IActionResult Create()
        {
            ViewData["CooperativeId"] = new SelectList(_context.Cooperatives, "CooperativeId", "CooperativeId");
            return View();
        }

        // POST: Rows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RowId,RowNumber,MaxGarageCount,CooperativeId")] Row row)
        {
            if (ModelState.IsValid)
            {
                _context.Add(row);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CooperativeId"] = new SelectList(_context.Cooperatives, "CooperativeId", "CooperativeId", row.CooperativeId);
            return View(row);
        }

        // GET: Rows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var row = await _context.Rows.FindAsync(id);
            if (row == null)
            {
                return NotFound();
            }
            ViewData["CooperativeId"] = new SelectList(_context.Cooperatives, "CooperativeId", "CooperativeId", row.CooperativeId);
            return View(row);
        }

        // POST: Rows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RowId,RowNumber,MaxGarageCount,CooperativeId")] Row row)
        {
            if (id != row.RowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(row);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RowExists(row.RowId))
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
            ViewData["CooperativeId"] = new SelectList(_context.Cooperatives, "CooperativeId", "CooperativeId", row.CooperativeId);
            return View(row);
        }

        // GET: Rows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var row = await _context.Rows
                .Include(r => r.Cooperative)
                .FirstOrDefaultAsync(m => m.RowId == id);
            if (row == null)
            {
                return NotFound();
            }

            return View(row);
        }

        // POST: Rows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var row = await _context.Rows.FindAsync(id);
            _context.Rows.Remove(row);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RowExists(int id)
        {
            return _context.Rows.Any(e => e.RowId == id);
        }
    }
}
