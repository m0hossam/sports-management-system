using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<StadiumManagersController> _logger;

        public SystemAdminsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<StadiumManagersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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

        // GET: SystemAdmins/Clubs
        public async Task<IActionResult> Clubs()
        {
            return _context.Clubs != null ?
            View(await _context.Clubs.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Clubs'  is null.");
        }

        // GET: SystemAdmins/DeleteClub/
        public async Task<IActionResult> DeleteClub(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FirstOrDefaultAsync(c => c.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: SystemAdmins/DeleteClubConfirmed/
        [HttpPost, ActionName(nameof(DeleteClub))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClubConfirmed(int id)
        {
            if (_context.Clubs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clubs'  is null.");
            }
            var club = await _context.Clubs.FindAsync(id);
            if (club != null)
            {
                // Delete club representative associated with club
                var clubRep = _context.ClubRepresentatives.Include(x => x.User).Include(x => x.Club).FirstOrDefault(x => x.Club == club);

                if (clubRep != null)
                {
                    var user = clubRep.User;

                    var result = await _userManager.DeleteAsync(user);
                    var userId = await _userManager.GetUserIdAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Unexpected error occurred deleting user.");
                    }

                    _logger.LogInformation("User with ID '{UserId}' was deleted because they represent a deleted club entity.", userId);
                }

                // Delete matches in which club participates/participated
                var matches = _context.Matches.Where(x => x.HomeClubId == club.Id || x.AwayClubId == club.Id);
                _context.Matches.RemoveRange(matches);

                // Delete club
                _context.Clubs.Remove(club);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Clubs));

        }

        // GET: SystemAdmins/AddClub
        public IActionResult AddClub()
        {
            return View();
        }

        // POST: SystemAdmins/AddClub
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
                return RedirectToAction(nameof(Clubs));
            }
            return View(club);
        }

        // GET: SystemAdmins/Stadiums
        public async Task<IActionResult> Stadiums()
        {
            return _context.Stadiums != null ?
            View(await _context.Stadiums.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Stadiums'  is null.");
        }

        // GET: SystemAdmins/DeleteStadium/
        public async Task<IActionResult> DeleteStadium(int? id)
        {
            if (id == null || _context.Stadiums == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums.FirstOrDefaultAsync(s => s.Id == id);

            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: SystemAdmins/DeleteStadiumConfirmed/
        [HttpPost, ActionName(nameof(DeleteStadium))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStadiumConfirmed(int id)
        {
            if (_context.Stadiums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stadiums'  is null.");
            }
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium != null)
            {
                _context.Stadiums.Remove(stadium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Stadiums));
        }

        // GET: SystemAdmins/AddStadium
        public IActionResult AddStadium()
        {
            return View();
        }

        // POST: SystemAdmins/AddStadium
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStadium([Bind("Id,Name,Location,Capacity")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                var duplicateStadium = _context.Stadiums.FirstOrDefault(x => x.Name == stadium.Name);
                if (duplicateStadium != null)
                {
                    ModelState.AddModelError(string.Empty, $"A stadium with the name of '{duplicateStadium.Name}' already exists.");
                    return View(stadium);
                }

                if(stadium.Capacity < 0)
                {
                    ModelState.AddModelError(string.Empty, $"Capacity shouldn't be negative.");
                    return View(stadium);
                }

                _context.Add(stadium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stadiums));
            }
            return View(stadium);
        }

        // GET: SystemAdmins/Fans
        public async Task<IActionResult> Fans()
        {
            return _context.Fans != null ?
            View(await _context.Fans.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Fans'  is null.");
        }

        // GET: SystemAdmins/FanDetails/
        public async Task<IActionResult> FanDetails(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FirstOrDefaultAsync(f => f.Id == id);

            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: SystemAdmins/BlockFan/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockFan(int id)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Fans'  is null.");
            }
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                fan.IsBlocked = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Fans));
        }

        // POST: SystemAdmins/UnblockFan/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockFan(int id)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Fans'  is null.");
            }
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                fan.IsBlocked = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Fans));
        }

    }
}