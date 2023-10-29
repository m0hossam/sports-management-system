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
            if (match == null)
            {
                return NotFound();
            }

            if (match.StartTime > DateTime.UtcNow)
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

        // GET: AssociationManagers/AddMatch
        public IActionResult AddMatch()
        {
            ViewData["HomeClubId"] = new SelectList(_context.Clubs, "Id", "Name");
            ViewData["AwayClubId"] = new SelectList(_context.Clubs, "Id", "Name");
            return View();
        }

        // POST: AssociationManagers/AddMatch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMatch([Bind("Id,HomeClubId,AwayClubId,StartTime,EndTime")] Match match)
        {
            bool isMatchPossible = true;

            if (match.HomeClubId == match.AwayClubId)
            {
                isMatchPossible = false;
                ModelState.AddModelError(string.Empty, "You have to choose two different clubs.");
            }
            if (match.EndTime <= match.StartTime)
            {
                isMatchPossible = false;
                ModelState.AddModelError(string.Empty, "The match's end time must be later than its start time.");
            }
            if (match.StartTime <= DateTime.Now)
            {
                isMatchPossible = false;
                ModelState.AddModelError(string.Empty, "The match's start time must be later than the current datetime.");
            }
            if (_context.Matches.Any())
            {
                if (_context.Matches.Where(x =>
                (x.HomeClubId == match.HomeClubId || x.AwayClubId == match.HomeClubId || x.HomeClubId == match.AwayClubId || x.AwayClubId == match.AwayClubId) &&
                !(match.StartTime > x.EndTime || match.EndTime < x.StartTime)).Any())
                {
                    isMatchPossible = false;
                    ModelState.AddModelError(string.Empty, "The clubs you chose have atleast one match with a conflicting time interval.");
                }
            }

            if (!isMatchPossible)
            {
                ViewData["HomeClubId"] = new SelectList(_context.Clubs, "Id", "Name", match.HomeClubId);
                ViewData["AwayClubId"] = new SelectList(_context.Clubs, "Id", "Name", match.AwayClubId);
                return View(match);
            }

            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["HomeClubId"] = new SelectList(_context.Clubs, "Id", "Name", match.HomeClubId);
            ViewData["AwayClubId"] = new SelectList(_context.Clubs, "Id", "Name", match.AwayClubId);
            return View(match);
        }

        // GET: AssociationManagers/ClubsNeverPaired
        public IActionResult ClubsNeverPaired() 
        {
            var pairs = from club1 in _context.Clubs
                        from club2 in _context.Clubs
                        where club1.Id < club2.Id
                        where !_context.Matches.Any(match => (match.HomeClubId == club1.Id && match.AwayClubId == club2.Id) ||
                        (match.HomeClubId == club2.Id && match.AwayClubId == club1.Id))
                        select new
                        {
                            FirstClubName = club1.Name,
                            SecondClubName = club2.Name
                        };

            List<Tuple<string, string>> pairsModel = new();
            foreach (var pair in pairs)
            {
                pairsModel.Add(new Tuple<string, string>(pair.FirstClubName, pair.SecondClubName));
            }

            return View(pairsModel);
        }

    }

}