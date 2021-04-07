using FlightManager.Data;
using FlightManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        [Authorize]
        public IActionResult Index(int? page, string searchString, string filter, int pageSize = 10)
        {
            int pageNumber = (page ?? 1);
            UsersIndexViewModel model = new UsersIndexViewModel()
            {
                Users = _context.Users.Select(u => new UserIndexViewModel
                {
                    UserId = u.Id,
                    Address = u.Address,
                    FirstName = u.FirstName,
                    Surname = u.Surname,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    SSN = u.SSN,
                    Username = u.UserName,
                    Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault().ToString()
                }).ToList(),
                Filter = filter,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = (int)(Math.Ceiling(_context.Users.Count() / (double)pageSize))
            };
            if (!String.IsNullOrEmpty(searchString))
            {
                model.Users = model.Users.Where(f => f.FirstName.Contains(searchString) || f.Surname.Contains(searchString) || 
                f.Email.Contains(searchString) || f.Address.Contains(searchString) || f.Username.Contains(searchString)).ToList();
            }
            switch (filter)
            {
                case "email":
                    model.Users = model.Users.OrderBy(u => u.Email).ToList();
                    break;
                case "emailReversed":
                    model.Users = model.Users.OrderByDescending(u => u.Email).ToList();
                    break;
                case "username":
                    model.Users = model.Users.OrderBy(u => u.Username).ToList();
                    break;
                case "usernameReversed":
                    model.Users = model.Users.OrderByDescending(u => u.Username).ToList();
                    break;
                case "firstName":
                    model.Users = model.Users.OrderBy(u => u.FirstName).ToList();
                    break;
                case "firstNameReversed":
                    model.Users = model.Users.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "lastName":
                    model.Users = model.Users.OrderBy(u => u.Surname).ToList();
                    break;
                case "lastNameReversed":
                    model.Users = model.Users.OrderByDescending(u => u.Surname).ToList();
                    break;
            }

            model.Users = model.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(model);
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,SSN,Address,Phone,Role,Id,UserName,PhoneNumber")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
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
                    if (!UserExists(user.Id))
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [Authorize(Roles = "Admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
