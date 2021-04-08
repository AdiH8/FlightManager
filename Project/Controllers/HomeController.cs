using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FlightManager.Data;
using FlightManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.roleManager = roleManager;
            this._context = applicationDbContext;
            this._userManager = userManager;
        }
         
        public async Task<IActionResult> Index()
        {
            IdentityRole userRole = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "User" };

            IdentityRole adminRole = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "Admin" };

            await this.roleManager.CreateAsync(userRole);
            await this.roleManager.CreateAsync(adminRole);

            ApplicationUser admin = new ApplicationUser()
            {
                FirstName = "Admin",
                Surname = "Admin",
                SSN = "0987654321",
                Address = "Bulgaria",
                PhoneNumber = "0987654321",
                UserName = "admin",
                EmailConfirmed = true,
                Email = "admin@email.com"
            };

            if (_userManager.Users.ToList().Count==0)
            {
                await _userManager.CreateAsync(admin, "Admin.1");
                await _userManager.AddToRoleAsync(admin, "Admin");
                
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
