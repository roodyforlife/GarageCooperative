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
    public class UsersController : Controller
    {
        private readonly DataBaseContext _context;

        public UsersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string passportNumber, string userName, string userPhone, string email,
            int salaryFrom, int salaryTo, UserSort sort = UserSort.SurnameAsc)
        {
            IQueryable<User> users = _context.Users;

            if (!String.IsNullOrEmpty(passportNumber))
            {
                users = users.Where(x => x.PassportNumber.Contains(passportNumber));
            }

            if (!String.IsNullOrEmpty(userName))
            {
                users = users.Where(x => x.Name.Contains(userName) || x.Surname.Contains(userName) || x.Lastname.Contains(userName));
            }

            if (!String.IsNullOrEmpty(userPhone))
            {
                users = users.Where(x => x.Telephone.Contains(userPhone));
            }

            if (!String.IsNullOrEmpty(email))
            {
                users = users.Where(x => x.Email.Contains(email));
            }

            if (salaryTo == 0 && users.Count() != 0)
            {
                salaryTo = users.Max(x => x.Salary);
            }

            users = users.Where(x => x.Salary >= salaryFrom);
            users = users.Where(x => x.Salary <= salaryTo);

            switch (sort)
            {
                case UserSort.NameAsc:
                    users = users.OrderBy(x => x.Name);
                    break;
                case UserSort.NameDesc:
                    users = users.OrderByDescending(x => x.Name);
                    break;
                case UserSort.SurnameDesc:
                    users = users.OrderByDescending(x => x.Surname);
                    break;
                case UserSort.LastNameAsc:
                    users = users.OrderBy(x => x.Lastname);
                    break;
                case UserSort.LastNameDesc:
                    users = users.OrderByDescending(x => x.Lastname);
                    break;
                case UserSort.TelephoneAsc:
                    users = users.OrderBy(x => x.Telephone);
                    break;
                case UserSort.EmailAsc:
                    users = users.OrderBy(x => x.Email);
                    break;
                case UserSort.PasportNumberAsc:
                    users = users.OrderBy(x => x.PassportNumber);
                    break;
                case UserSort.SalaryAsc:
                    users = users.OrderBy(x => x.Salary);
                    break;
                case UserSort.SalaryDesc:
                    users = users.OrderByDescending(x => x.Salary);
                    break;
                default:
                    users = users.OrderBy(x => x.Surname);
                    break;
            }

            ViewBag.PassportNumber = passportNumber;
            ViewBag.UserName = userName;
            ViewBag.UserPhone = userPhone;
            ViewBag.Email = email;
            ViewBag.SalaryFrom = salaryFrom;
            ViewBag.SalaryTo = salaryTo;
            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(UserSort)).Cast<UserSort>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Surname,Lastname,Telephone,Email,PassportNumber,Address,Salary")] User user)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Telephone == user.Telephone) is not null)
            {
                ModelState.AddModelError("Telephone", "Account has been registered");
            }

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Surname,Lastname,Telephone,Email,PassportNumber,Address,Salary")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (await _context.Users.FirstOrDefaultAsync(x => x.Telephone == user.Telephone && x.UserId != user.UserId) is not null)
            {
                ModelState.AddModelError("Telephone", "Account has been registered");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
