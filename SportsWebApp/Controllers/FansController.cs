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
    [Authorize(Roles = "Fan")]
    public class FansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FansController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Fans
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var fan = _context.Fans.FirstOrDefault(x => x.User == user);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        // GET: Fan/ViewAvailableMatches
        public async Task<IActionResult> ViewAvailableMatches()
        {
            return _context.Matches != null ?
            View(await _context.Matches
            .Include(x => x.HomeClub)
            .Include(x => x.AwayClub)
            .Include(x => x.Stadium)
            .Where(x => x.StartTime > DateTime.Now && x.Stadium != null && x.NumberOfAttendees < x.Stadium.Capacity)
            .OrderBy(x => x.StartTime)
            .ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
        }

        // GET: Fans/PurchaseTicket/
        public async Task<IActionResult> PurchaseTicket(int? id)
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

            return View(match);
        }

        // POST: Fans/PurchaseTicket/
        [HttpPost, ActionName(nameof(PurchaseTicket))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseTicketConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var fan = _context.Fans.FirstOrDefault(x => x.User == user);
            if (fan == null)
            {
                return NotFound();
            }

            if (fan.IsBlocked)
            {
                TempData["Message"] = "You are blocked by a system admin.";
                return RedirectToAction(nameof(ViewAvailableMatches));
            }

            var match = await _context.Matches.Include(x => x.Stadium).FirstOrDefaultAsync(x => x.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            if (match.StartTime <= DateTime.Now || match.Stadium == null || match.NumberOfAttendees >= match.Stadium.Capacity)
            {
                TempData["Message"] = "Your transaction could not be completed.";
                return RedirectToAction(nameof(ViewAvailableMatches));
            }

            match.NumberOfAttendees++;
            var ticket = new Ticket { FanId = fan.Id, MatchId = id, Fan = fan };
            _context.Add(ticket);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewAllTickets));
        }


        public async Task<IActionResult> ViewAllTickets()
        {
            var user = await _userManager.GetUserAsync(User);
            var fan = _context.Fans.FirstOrDefault(x => x.User == user);

            if (fan == null)
            {
                return NotFound();
            }

            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets' is null.");
            }

            return View(await _context.Tickets
                .Include(x => x.Match)
                .ThenInclude(x => x!.HomeClub)
                .Include(x => x.Match)
                .ThenInclude(x => x!.AwayClub)
                .Include(x => x.Match)
                .ThenInclude(x => x!.Stadium)
                .Where(x => x.FanId == fan.Id)
                .ToListAsync());
        }
    }
}
