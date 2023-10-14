﻿using System;
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


        // GET: ViewStadiumInfo
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

        public async Task<IActionResult> ViewRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);

            if(stadiumManager == null || _context.HostRequests == null) { return NotFound(); }

            var hostrequest=await _context.HostRequests.Include(x=>x.ClubRepresentative).Include(x=>x.Match).Where(x=>x.StadiumId==stadiumManager.StadiumId).ToListAsync();

            return View(hostrequest);
        }



    }
}
