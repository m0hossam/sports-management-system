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
    [Authorize(Roles = "System Admin")]
    public class SystemAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SystemAdminsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SystemAdmins
        public IActionResult Index()
        {
            var userTask = _userManager.GetUserAsync(User);
            var systemAdmin = _context.SystemAdmins.Where(x => x.User == userTask.Result).FirstOrDefault();
            return View(systemAdmin);
        }

        // GET: SystemAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SystemAdmins == null)
            {
                return NotFound();
            }

            var systemAdmin = await _context.SystemAdmins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemAdmin == null)
            {
                return NotFound();
            }

            return View(systemAdmin);
        }

        // GET: SystemAdmins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SystemAdmin systemAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemAdmin);
        }

        // GET: SystemAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SystemAdmins == null)
            {
                return NotFound();
            }

            var systemAdmin = await _context.SystemAdmins.FindAsync(id);
            if (systemAdmin == null)
            {
                return NotFound();
            }
            return View(systemAdmin);
        }

        // POST: SystemAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SystemAdmin systemAdmin)
        {
            if (id != systemAdmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemAdminExists(systemAdmin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(systemAdmin);
        }

        // GET: SystemAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SystemAdmins == null)
            {
                return NotFound();
            }

            var systemAdmin = await _context.SystemAdmins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemAdmin == null)
            {
                return NotFound();
            }

            return View(systemAdmin);
        }

        // POST: SystemAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SystemAdmins == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SystemAdmins'  is null.");
            }
            var systemAdmin = await _context.SystemAdmins.FindAsync(id);
            if (systemAdmin != null)
            {
                _context.SystemAdmins.Remove(systemAdmin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemAdminExists(int id)
        {
          return (_context.SystemAdmins?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
