using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Data;
using SportsWebApp.Models;

namespace SportsWebApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly SportsWebAppContext _context;

        public RegisterController(SportsWebAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Verifies that a given username is not already taken
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUsername(string username)
        {
            bool usernameTaken = _context.SystemAdmin.Any(x => x.Username == username);
            if (usernameTaken)
            {
                return Json($"{username} is already taken.");
            }

            return Json(true);
        }

        // GET: Register/SystemAdminRegister
        public IActionResult SystemAdminRegister()
        {
            return View();
        }

        // POST: Register/SystemAdminRegister
        // Registers a new System Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SystemAdminRegister([Bind("Id,Username,Password")] SystemAdmin systemAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "SystemAdmins", new {id = systemAdmin.Id});
            }
            return View(systemAdmin);
        }
    }
}
