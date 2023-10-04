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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<StadiumManagersController> _logger;

        public StadiumManagersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<StadiumManagersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: StadiumManagers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.User == user);
            if (stadiumManager == null)
            {
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var result = await _userManager.DeleteAsync(user);
                var userId = await _userManager.GetUserIdAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred deleting user.");
                }

                await _signInManager.SignOutAsync();

                _logger.LogInformation("User with ID '{UserId}' was deleted due to a dependency on a deleted stadium entity.", userId);

                return NotFound();
            }
            return View(stadiumManager);
        }
    }
}
