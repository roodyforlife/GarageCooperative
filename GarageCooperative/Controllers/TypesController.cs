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
    public class TypesController : Controller
    {
        private readonly DataBaseContext _context;

        public TypesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            return View(await _context.Types.ToListAsync());
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,Name,Cost,GarbadgeCost,WaterCost,FloorNumber")] GarageCooperative.Models.Type @type)
        {
            if (await _context.Types.FirstOrDefaultAsync(x => x.Name == type.Name) is not null)
            {
                ModelState.AddModelError("Name", "Type already taken");
            }

            if (ModelState.IsValid)
            {
                _context.Add(@type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,Name,Cost,GarbadgeCost,WaterCost,FloorNumber")] GarageCooperative.Models.Type @type)
        {
            if (id != @type.TypeId)
            {
                return NotFound();
            }

            if (await _context.Types.FirstOrDefaultAsync(x => x.Name == type.Name && x.TypeId != type.TypeId) is not null)
            {
                ModelState.AddModelError("Name", "Type already taken");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.TypeId))
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
            return View(@type);
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @type = await _context.Types.FindAsync(id);
            _context.Types.Remove(@type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(int id)
        {
            return _context.Types.Any(e => e.TypeId == id);
        }
    }
}
