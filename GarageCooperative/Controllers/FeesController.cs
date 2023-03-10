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
    public class FeesController : Controller
    {
        private readonly DataBaseContext _context;

        public FeesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Fees
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Fees
                .Include(f => f.Garage)
                .ThenInclude(x => x.Row)
                .ThenInclude(x => x.Cooperative);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Fees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees
                .Include(f => f.Garage)
                .ThenInclude(x => x.Row)
                .ThenInclude(x => x.Cooperative)
                .FirstOrDefaultAsync(m => m.FeeId == id);
            if (fee == null)
            {
                return NotFound();
            }

            return View(fee);
        }

        // GET: Fees/Create
        public IActionResult Create()
        {
            var garages = _context.Garages.Include(x => x.Row).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber})"
                });
            }

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text");
            return View();
        }

        // POST: Fees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeeId,GarageId,Date,Payment")] Fee fee)
        {
            fee.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(fee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var garages = _context.Garages.Include(x => x.Row).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber})"
                });
            }

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text", fee.GarageId);
            return View(fee);
        }

        // GET: Fees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees.FindAsync(id);
            if (fee == null)
            {
                return NotFound();
            }

            var garages = _context.Garages.Include(x => x.Row).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber})"
                });
            }

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text", fee.GarageId);
            return View(fee);
        }

        // POST: Fees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeeId,GarageId,Date,Payment")] Fee fee)
        {
            if (id != fee.FeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeeExists(fee.FeeId))
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

            var garages = _context.Garages.Include(x => x.Row).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber})"
                });
            }

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text", fee.GarageId);
            return View(fee);
        }

        // GET: Fees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees
                .Include(f => f.Garage)
                .FirstOrDefaultAsync(m => m.FeeId == id);
            if (fee == null)
            {
                return NotFound();
            }

            return View(fee);
        }

        // POST: Fees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fee = await _context.Fees.FindAsync(id);
            _context.Fees.Remove(fee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeeExists(int id)
        {
            return _context.Fees.Any(e => e.FeeId == id);
        }
    }
}
