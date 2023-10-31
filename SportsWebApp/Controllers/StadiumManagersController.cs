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
    [Authorize(Roles = "Stadium Manager")]
    public class StadiumManagersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StadiumManagersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: StadiumManagers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);
            if (stadiumManager == null)
            {
                return NotFound();
            }
            return View(stadiumManager);
        }


        // GET: StadiumManagers/ViewStadiumInfo
        public async Task<IActionResult> ViewStadiumInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.Include(x => x.Stadium).FirstOrDefault(x => x.User == user);
            if (stadiumManager == null)
            {
                return NotFound();
            }
            return View(stadiumManager);
        }


        // GET: StadiumManagers/ViewRequests
        public async Task<IActionResult> ViewRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            if (stadiumManager == null)
            {
                return NotFound();
            }

            if (_context.HostRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HostRequests'  is null.");
            }

            var hostRequests = await _context.HostRequests
                .Include(x => x.ClubRepresentative)
                .Include(x => x.Stadium)
                .Include(x => x.Match)
                .ThenInclude(x => x!.HomeClub)
                .Include(x => x.Match)
                .ThenInclude(x => x!.AwayClub)
                .Where(x => x.StadiumId == stadiumManager.StadiumId)
                .ToListAsync();

            return View(hostRequests);
        }

        // POST: StadiumManagers/AcceptRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            var hostRequest = await _context.HostRequests.FindAsync(id);
            if (stadiumManager == null || hostRequest == null) 
            { 
                return NotFound(); 
            }

            if (hostRequest.IsApproved != null)
            {
                TempData["Message"] = "The request you tried to handle has been already handled.";
                return RedirectToAction(nameof(ViewRequests));
            }

            var match = await _context.Matches.FindAsync(hostRequest.MatchId);
            if (match == null)
            {
                return NotFound();
            }

            match.StadiumId = stadiumManager.StadiumId;
            hostRequest.IsApproved = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewRequests));
        }


        // POST: StadiumManagers/RejectRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            var hostRequest = await _context.HostRequests.FindAsync(id);
            if (stadiumManager == null || hostRequest == null)
            {
                return NotFound();
            }

            if (hostRequest.IsApproved != null)
            {
                TempData["Message"] = "The request you tried to handle has been already handled.";
                return RedirectToAction(nameof(ViewRequests));
            }

            var match = await _context.Matches.FindAsync(hostRequest.MatchId);
            if (match == null)
            {
                return NotFound();
            }

            match.StadiumId = stadiumManager.StadiumId;
            hostRequest.IsApproved = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewRequests));
        }
    }
}
