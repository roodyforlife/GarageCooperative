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
    public class CooperativesController : Controller
    {
        private readonly DataBaseContext _context;

        public CooperativesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Cooperatives
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cooperatives.ToListAsync());
        }

        // GET: Cooperatives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cooperative = await _context.Cooperatives
                .FirstOrDefaultAsync(m => m.CooperativeId == id);
            if (cooperative == null)
            {
                return NotFound();
            }

            return View(cooperative);
        }

        // GET: Cooperatives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cooperatives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CooperativeId,Name,Address,PostCode,Telephone")] Cooperative cooperative)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cooperative);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cooperative);
        }

        // GET: Cooperatives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cooperative = await _context.Cooperatives.FindAsync(id);
            if (cooperative == null)
            {
                return NotFound();
            }
            return View(cooperative);
        }

        // POST: Cooperatives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CooperativeId,Name,Address,PostCode,Telephone")] Cooperative cooperative)
        {
            if (id != cooperative.CooperativeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cooperative);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CooperativeExists(cooperative.CooperativeId))
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
            return View(cooperative);
        }

        // GET: Cooperatives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cooperative = await _context.Cooperatives
                .FirstOrDefaultAsync(m => m.CooperativeId == id);
            if (cooperative == null)
            {
                return NotFound();
            }

            return View(cooperative);
        }

        // POST: Cooperatives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cooperative = await _context.Cooperatives.FindAsync(id);
            _context.Cooperatives.Remove(cooperative);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CooperativeExists(int id)
        {
            return _context.Cooperatives.Any(e => e.CooperativeId == id);
        }
    }
}
