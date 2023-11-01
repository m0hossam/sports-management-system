using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Data;
using SportsWebApp.Models;

namespace SportsWebApp.Controllers
{
    [Authorize(Roles = "Club Representative")]
    public class ClubRepresentativesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClubRepresentativesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ClubRepresentatives
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var clubRep = _context.ClubRepresentatives.FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }
            return View(clubRep);
        }


        // GET: ClubRepresentatives/AddHostRequest/
        public async Task<IActionResult> AddHostRequest(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var clubRep = _context.ClubRepresentatives.FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }

            var match = _context.Matches.Where(x => x.Id == id).FirstOrDefault();
            if (match == null)
            {
                return NotFound();
            }

            ViewData["ClubRepresentativeId"] = clubRep.Id;
            ViewData["MatchId"] = match.Id;
            ViewData["StadiumId"] = new SelectList(_context.Stadiums, "Id", "Name");
            return View();
        }

        // POST: ClubRepresentatives/AddHostRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHostRequest([Bind("ClubRepresentativeId,MatchId,StadiumId")] HostRequest hostRequest)
        {

            if (ModelState.IsValid)
            {
                if (!_context.HostRequests.Any(x => 
                x.ClubRepresentativeId == hostRequest.ClubRepresentativeId &&
                x.MatchId == hostRequest.MatchId &&
                x.StadiumId == hostRequest.StadiumId))
                {
                    _context.Add(hostRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                TempData["Message"] = "There already exists a host request with the same information.";
            }

            ViewData["ClubRepresentativeId"] = hostRequest.ClubRepresentativeId;
            ViewData["MatchId"] = hostRequest.MatchId;
            ViewData["StadiumId"] = new SelectList(_context.Stadiums, "Id", "Name", hostRequest.StadiumId);
            return View(hostRequest);
        }


        // GET: ClubRepresentatives/ViewClubInfo
        public async Task<IActionResult> ViewClubInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            var clubRep = _context.ClubRepresentatives.FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }

            var club = _context.Clubs.FirstOrDefault(x => x.Id == clubRep.ClubId);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }


        // GET: ClubRepresentatives/ViewAvailableStadiumsForm
        public IActionResult ViewAvailableStadiumsForm()
        {
            return View();
        }

        // GET: ClubRepresentatives/ViewAvailableStadiums/
        public async Task<IActionResult> ViewAvailableStadiums(DateTime? startTime, DateTime? endTime)
        {
            if (_context.Stadiums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stadiums'  is null.");
            }

            if (startTime == null || endTime == null)
            {
                TempData["Message"] = "You have to enter a starttime and an endtime.";
                return ViewAvailableStadiumsForm();
            }

            if (startTime <= DateTime.UtcNow)
            {
                TempData["Message"] = "Please choose a starttime that is later than the current time.";
                return ViewAvailableStadiumsForm();
            }

            if (startTime >= endTime)
            {
                TempData["Message"] = "Please choose an endtime that is later than the starttime.";
                return ViewAvailableStadiumsForm();
            }

            var stadiums = await _context.Stadiums
                .Where(s => !_context.Matches.Any(m=> m.StadiumId==s.Id && !(endTime<m.StartTime || startTime>m.EndTime)))
                .ToListAsync();

            return View(stadiums);
        }

        // GET: ClubRepresentatives/ViewUpcomingMatches
        public async Task<IActionResult> ViewUpcomingMatches()
        {
            var user = await _userManager.GetUserAsync(User);
            var clubRep = _context.ClubRepresentatives.Include(x => x.Club).FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }

            if (_context.Matches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
            }

            var matches = await _context.Matches
                .Include(x=>x.AwayClub)
                .Include(x=>x.HomeClub)
                .Include(x=>x.Stadium)
                .Where(x => x.StartTime > DateTime.UtcNow && x.HomeClubId == clubRep.ClubId)
                .ToListAsync();

            if (matches == null) 
            {
                return NotFound(); 
            }

            return View(matches);
        }

        // GET: ClubRepresentatives/ViewSentRequests
        public async Task<IActionResult> ViewSentRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            var clubRep = _context.ClubRepresentatives.FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }

            if (_context.HostRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HostRequests'  is null.");
            }

            var hostRequests = await _context.HostRequests
                .Include(x => x.Match)
                .ThenInclude(x => x!.HomeClub)
                .Include(x => x.Match)
                .ThenInclude(x => x!.AwayClub)
                .Include(x => x.Stadium)
                .Where(x => x.ClubRepresentativeId == clubRep.Id)
                .ToListAsync();

            return View(hostRequests);
        }
    }
}
