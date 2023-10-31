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


        // POST: ViewStadiumInfo
        public async Task<IActionResult> ViewStadiumInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.Include(x=>x.Stadium).FirstOrDefault(x => x.User == user);
            if (stadiumManager == null)
            {
                return NotFound();
            }
            return View(stadiumManager);
        }


        // POST: ViewRequests
        public async Task<IActionResult> ViewRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            if(stadiumManager == null || _context.HostRequests == null) return NotFound();

            var hostrequest=await _context.HostRequests.Include(x=>x.ClubRepresentative).Include(x=>x.Stadium).Include(x=>x.Match.HomeClub).Include(x=>x.Match.AwayClub).Where(x=>x.StadiumId==stadiumManager.StadiumId).ToListAsync();

            return View(hostrequest);
        }

        // POST: AcceptRequest
        public async Task<IActionResult> AcceptRequest(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            var hostrequest = await _context.HostRequests.FindAsync(id);

            if(stadiumManager == null || hostrequest==null) return NotFound();

            var match = await _context.Matches.FindAsync(hostrequest.MatchId);
            
            if (match==null||match.StadiumId!=null) return NotFound();//if match was assigned 

            match.StadiumId = stadiumManager.StadiumId;
            match.Stadium = stadiumManager.Stadium;
            hostrequest.IsApproved = true;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewRequests));
        }


        // POST: RejectRequest
        public async Task<IActionResult> RejectRequest(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            var hostrequest = await _context.HostRequests.FindAsync(id);

            if (stadiumManager == null || hostrequest == null) return NotFound();

            hostrequest.IsApproved = false;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewRequests));
        }
    }
}
