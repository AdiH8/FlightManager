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
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.roleManager = roleManager;
            this._context = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            IdentityRole userRole = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "User" };

            IdentityRole adminRole = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "Admin" };


            await this.roleManager.CreateAsync(userRole);
            await this.roleManager.CreateAsync(adminRole);

            return View(await _context.Flights.ToListAsync());
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
