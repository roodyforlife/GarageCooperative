using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageCooperative.DataBase;
using GarageCooperative.Models;
using GarageCooperative.Enums;

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
        public async Task<IActionResult> Index(int garageNumber, string userName, string userPhone,
            DateTime ownStartFrom, DateTime ownStartTo, MembershipSort sort = MembershipSort.GarageNumberAsc)
        {
            IQueryable<Membership> dataBaseContext = _context.Memberships.Include(m => m.Garage)
                .ThenInclude(x => x.Row)
                .ThenInclude(x => x.Cooperative)
                .Include(m => m.User);

            if (garageNumber != 0)
            {
                dataBaseContext = dataBaseContext.Where(x => x.Garage.Number == garageNumber);
            }

            if (!String.IsNullOrEmpty(userName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.User.Name.Contains(userName) || x.User.Surname.Contains(userName) ||
                x.User.Lastname.Contains(userName));
            }

            if (!String.IsNullOrEmpty(userPhone))
            {
                dataBaseContext = dataBaseContext.Where(x => x.User.Telephone.Contains(userPhone));
            }

            if (ownStartTo.Year == 1)
            {
                ownStartTo = DateTime.Now.AddDays(1);
            }

            dataBaseContext = dataBaseContext.Where(x => x.OwnStart >= ownStartFrom);
            dataBaseContext = dataBaseContext.Where(x => x.OwnStart <= ownStartTo);

            switch (sort)
            {
                case MembershipSort.GarageNumberDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Garage.Number);
                    break;
                case MembershipSort.UserNameAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.User.Name);
                    break;
                case MembershipSort.UserNameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.User.Name);
                    break;
                case MembershipSort.UserPhoneAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.User.Telephone);
                    break;
                case MembershipSort.UserPhoneDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.User.Telephone);
                    break;
                case MembershipSort.OwnStartAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.OwnStart);
                    break;
                case MembershipSort.OwnEndAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.OwnEnd);
                    break;
                default:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Garage.Number);
                    break;
            }

            ViewBag.GarageNumber = garageNumber;
            ViewBag.UserName = userName;
            ViewBag.UserPhone = userPhone;
            ViewBag.OwnStartFrom = ownStartFrom;
            ViewBag.OwnStartTo = ownStartTo;
            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(MembershipSort)).Cast<MembershipSort>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

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
            var garages = _context.Garages.Include(x => x.Row).ThenInclude(x => x.Cooperative).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber} - {garage.Row.Cooperative.Name})"
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

            var garages = _context.Garages.Include(x => x.Row).ThenInclude(x => x.Cooperative).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber} - {garage.Row.Cooperative.Name})"
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

            var garages = _context.Garages.Include(x => x.Row).ThenInclude(x => x.Cooperative).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber} - {garage.Row.Cooperative.Name})"
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
                if (membership.OwnEnd is null)
                {
                    membership.OwnEnd = new DateTime();
                }

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

            var garages = _context.Garages.Include(x => x.Row).ThenInclude(x => x.Cooperative).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Garage garage in garages)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = garage.GarageId.ToString(),
                    Text = $"Garage number: {garage.Number} (Row: {garage.Row.RowNumber} - {garage.Row.Cooperative.Name})"
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
