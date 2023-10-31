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


        // POST: ViewStadiumInfo
        public async Task<IActionResult> ViewClubInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var clubRep = _context.ClubRepresentatives.Include(x=>x.Club).FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }

            return View(clubRep);
        }


        // GET: ViewAvailableStadiums

        public async Task<IActionResult> ViewAvailableStadiumsForm()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewAvailableStadiums(DateTime starttime, DateTime endtime)
        {
            var stadiium = _context.Stadiums.Where(s => !_context.Matches.Any(m=> m.StadiumId==s.Id && !(endtime<m.StartTime || starttime>m.EndTime)));
            return View(stadiium);
        }



        public async Task<IActionResult> ViewUpcomingMatches()
        {
            var matches =await _context.Matches.Include(x=>x.AwayClub).Include(x=>x.HomeClub).Include(x=>x.Stadium).Where(x => x.StartTime >= DateTime.UtcNow).ToListAsync();
            if (matches == null) return NotFound();
            return View(matches);
        }

        public async Task<IActionResult> ViewSentRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var clubRep = _context.ClubRepresentatives.FirstOrDefault(x => x.User == user);
            if (clubRep == null)
            {
                return NotFound();
            }

            var hostRequest = _context.HostRequests.Include(x=>x.Match.HomeClub).Include(x => x.Match.AwayClub).Include(x=>x.Stadium).Where(x => x.ClubRepresentativeId== clubRep.Id);
            return View(hostRequest);
        }
    }
}
