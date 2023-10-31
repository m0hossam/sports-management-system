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
            return View();
        }



        public async Task<IActionResult> ViewUpcomingMatches()
        {
            var matches =await _context.Matches.Include(x=>x.AwayClub).Include(x=>x.HomeClub).Include(x=>x.Stadium).Where(x => x.StartTime >= DateTime.UtcNow).ToListAsync();
            if (matches == null) return NotFound();
            return View(matches);
        }


    }
}
