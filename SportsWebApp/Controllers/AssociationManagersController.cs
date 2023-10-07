using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    [Authorize(Roles = "Association Manager")]
    public class AssociationManagersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AssociationManagersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AssociationManagers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var associationManager = _context.AssociationManagers.FirstOrDefault(x => x.User == user);
            if (associationManager == null)
            {
                return NotFound();
            }
            return View(associationManager);
        }

        // GET: AssociationManagers/UpcomingMatches
        public async Task<IActionResult> UpcomingMatches()
        {
            return _context.Matches != null ?
            View(await _context.Matches
            .Include(x => x.HomeClub)
            .Include(x => x.AwayClub)
            .Include(x => x.Stadium)
            .Where(x => x.StartTime > DateTime.UtcNow)
            .OrderBy(x => x.StartTime)
            .ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
        }

        // GET: AssociationManagers/DeleteMatch/
        public async Task<IActionResult> DeleteMatch(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(x => x.HomeClub)
                .Include(x => x.AwayClub)
                .Include(x => x.Stadium)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            if (match.StartTime <= DateTime.UtcNow) // match already started
            {
                return RedirectToAction(nameof(UpcomingMatches));
            }

            return View(match);
        }

        // POST: AssociationManagers/DeleteMatchConfirmed/
        [HttpPost, ActionName(nameof(DeleteMatch))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMatchConfirmed(int id)
        {
            if (_context.Matches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
            }
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UpcomingMatches));

        }

        // GET: AssociationManagers/OngoingMatches
        public async Task<IActionResult> OngoingMatches()
        {
            return _context.Matches != null ?
            View(await _context.Matches
            .Include(x => x.HomeClub)
            .Include(x => x.AwayClub)
            .Include(x => x.Stadium)
            .Where(x => x.StartTime <= DateTime.UtcNow && x.EndTime > DateTime.UtcNow)
            .OrderBy(x => x.StartTime)
            .ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
        }

        // GET: AssociationManagers/FinishedMatches
        public async Task<IActionResult> FinishedMatches()
        {
            return _context.Matches != null ?
            View(await _context.Matches
            .Include(x => x.HomeClub)
            .Include(x => x.AwayClub)
            .Include(x => x.Stadium)
            .Where(x => x.EndTime <= DateTime.UtcNow)
            .OrderByDescending(x => x.StartTime)
            .ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
        }
    }
}
