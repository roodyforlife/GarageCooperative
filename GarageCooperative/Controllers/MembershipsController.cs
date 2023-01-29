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
    public class MembershipsController : Controller
    {
        private readonly DataBaseContext _context;

        public MembershipsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Memberships
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Memberships.Include(m => m.Garage).Include(m => m.User);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Memberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .Include(m => m.Garage)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Memberships/Create
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Telephone");
            return View();
        }

        // POST: Memberships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,GarageId,UserId,OwnStart,OwnEnd")] Membership membership)
        {
            Membership DbMembership = await _context.Memberships.FirstOrDefaultAsync(x => x.GarageId == membership.GarageId);
            if (DbMembership is not null)
            {
                if (DbMembership.OwnEnd.Value.Year == 1 || DbMembership.OwnEnd >= DateTime.Now)
                {
                    ModelState.AddModelError("GarageId", "You can't add owner for this garage");
                }
            }

            if (membership.OwnEnd is null)
            {
                membership.OwnEnd = new DateTime();
            }

            if (ModelState.IsValid)
            {
                _context.Add(membership);
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

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text", membership.GarageId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Telephone", membership.UserId);
            return View(membership);
        }

        // GET: Memberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null)
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

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text", membership.GarageId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Telephone", membership.UserId);
            return View(membership);
        }

        // POST: Memberships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipId,GarageId,UserId,OwnStart,OwnEnd")] Membership membership)
        {
            if (id != membership.MembershipId)
            {
                return NotFound();
            }

            Membership DbMembership = await _context.Memberships.FirstOrDefaultAsync(x => x.GarageId == membership.GarageId
                && x.MembershipId != membership.MembershipId);
            if (DbMembership is not null)
            {
                if (DbMembership.OwnEnd.Value.Year == 1 || DbMembership.OwnEnd >= DateTime.Now)
                {
                    ModelState.AddModelError("GarageId", "You can't add owner for this garage");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.MembershipId))
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

            ViewData["GarageId"] = new SelectList(selectListItems, "Value", "Text", membership.GarageId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Telephone", membership.UserId);
            return View(membership);
        }

        // GET: Memberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .Include(m => m.Garage)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);
            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return _context.Memberships.Any(e => e.MembershipId == id);
        }
    }
}
