using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Data;
using SportsWebApp.Models;

namespace SportsWebApp.Controllers
{
    [Authorize(Roles = "System Admin")]
    public class SystemAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SystemAdminsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SystemAdmins
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var systemAdmin = _context.SystemAdmins.FirstOrDefault(x => x.User == user);
            if (systemAdmin == null)
            {
                return NotFound();
            }
            return View(systemAdmin);
        }

        // GET: SystemAdmins/AddClub
        public IActionResult AddClub()
        {
            return View();
        }

        // POST: SystemAdmins/AddClub
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClub([Bind("Id,Name,Location")] Club club)
        {
            if (ModelState.IsValid)
            {
                var duplicateClub = _context.Clubs.FirstOrDefault(x => x.Name == club.Name);
                if (duplicateClub != null)
                {
                    ModelState.AddModelError(string.Empty, $"A club with the name of '{duplicateClub.Name}' already exists.");
                    return View(club);
                }

                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"Club '{club.Name}' was created successfully." });
            }
            return View(club);
        }

        // GET: SystemAdmins/DeleteClub
        public IActionResult DeleteClub()
        {
            return View();
        }

        // POST: SystemAdmins/DeleteClub
        [HttpPost, ActionName("DeleteClub")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClub([Bind("Name")] Club wantedClub)
        {
            if (_context.Clubs == null)
            {
                return Problem("Entity set 'SportsWebAppContext.Clubs'  is null.");
            }

            var club = _context.Clubs.FirstOrDefault(x => x.Name == wantedClub.Name);
            if (club == null)
            {
                ModelState.AddModelError(string.Empty, $"There exists no club with the name of {wantedClub.Name}.");
                return View(wantedClub);
            }

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"Club '{club.Name}' was deleted successfully." });
        }

        // GET: SystemAdmins/SuccessfulOperation
        public IActionResult SuccessfulOperation(string? msg)
        {
            TempData["Success Message"] = (msg != null) ? msg : "";
            return View();
        }
    }
}