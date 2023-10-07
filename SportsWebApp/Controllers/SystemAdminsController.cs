﻿using System;
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

        // GET: SystemAdmins/AddClub
        public IActionResult AddClub()
        {
            return View();
        }

        // POST: SystemAdmins/AddClub
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"Club '{club.Name}' was created successfully." });
            }
            return View(club);
        }

        // GET: SystemAdmins/DeleteClub
        public IActionResult DeleteClub()
        {
            return View();
        }

        // POST: SystemAdmins/DeleteClub
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClub([Bind("Name")] Club wantedClub)
        {
            if (_context.Clubs == null)
            {
                return Problem("Entity set 'SportsWebAppContext.Clubs'  is null.");
            }

            var club = _context.Clubs.FirstOrDefault(x => x.Name == wantedClub.Name);
            if (club == null)
            {
                ModelState.AddModelError(string.Empty, $"There exists no club with the name of {wantedClub.Name}.");
                return View(wantedClub);
            }

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

            // Remove matches in which club participates/participated
            var matches = _context.Matches.Where(x => x.HomeClub == club || x.AwayClub == club);
            _context.Matches.RemoveRange(matches);

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"Club '{club.Name}' was deleted successfully." });
        }


        // GET: SystemAdmins/AddStadium
       
        public IActionResult AddStadium()
        {
            return View();
        }

        // POST: SystemAdmins/AddStadium
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"Stadium '{stadium.Name}' was created successfully." });
            }
            return View(stadium);
        }

        //
        /// <summary>
        /// </summary>
        /// <returns></returns>
        // GET: SystemAdmins/DeleteStadium
        public IActionResult DeleteStadium()
        {
            return View();
        }

        // POST: SystemAdmins/DeleteStadium
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStadium([Bind("Name")] Stadium wantedStadium)
        {
            if (_context.Stadiums == null)
            {
                return Problem("Entity set 'SportsWebAppContext.Stadiums'  is null.");
            }

            var stadium = _context.Stadiums.FirstOrDefault(x => x.Name == wantedStadium.Name);
            if (stadium == null)
            {
                ModelState.AddModelError(string.Empty, $"There exists no stadium with the name of {wantedStadium.Name}.");
                return View(wantedStadium);
            }

            // Delete stadium manager associated with stadium
            var stadiumManager = _context.StadiumManagers.Include(x => x.User).Include(x => x.Stadium).FirstOrDefault(x => x.Stadium == stadium);

            if (stadiumManager != null)
            {
                var user = stadiumManager.User;

                var result = await _userManager.DeleteAsync(user);
                var userId = await _userManager.GetUserIdAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred deleting user.");
                }

                _logger.LogInformation("User with ID '{UserId}' was deleted because they manage a deleted stadium entity.", userId);
            }

            _context.Stadiums.Remove(stadium);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"Stadium '{stadium.Name}' was deleted successfully." });
        }

        // GET: SystemAdmins/BlockFan
        public IActionResult BlockFan()
        {
            return View();
        }

        // POST: SystemAdmins/BlockFan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockFan([Bind("NationalId")] Fan wantedFan)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'SportsWebAppContext.Fans'  is null.");
            }

            var fan = _context.Fans.FirstOrDefault(x => x.NationalId == wantedFan.NationalId);
            if (fan == null)
            {
                ModelState.AddModelError(string.Empty, $"There exists no fan with a national ID '{wantedFan.NationalId}'.");
                return View(wantedFan);
            }

            fan.IsBlocked = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"The fan with the national ID of '{fan.NationalId}' was successfully blocked." });
        }

        // GET: SystemAdmins/UnblockFan
        public IActionResult UnblockFan()
        {
            return View();
        }

        //Post: SystemAdmins/UnblockFan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockFan([Bind("NationalId")] Fan wantedFan)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'SportsWebAppContext.Fans'  is null.");
            }
            var fan = _context.Fans.FirstOrDefault(x => x.NationalId == wantedFan.NationalId);
            if (fan == null)
            {
                ModelState.AddModelError(string.Empty, $"There exists no fan with a national ID '{wantedFan.NationalId}'.");
                return View(wantedFan);
            }

            fan.IsBlocked = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SuccessfulOperation), new { msg = $"The fan with the national ID of '{fan.NationalId}' was successfully unblocked." });

        }



        // GET: SystemAdmins/SuccessfulOperation
        public IActionResult SuccessfulOperation(string? msg)
        {
            TempData["Success Message"] = (msg != null) ? msg : "";
            return View();
        }
    }
}