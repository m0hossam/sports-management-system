using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Data;
using SportsWebApp.Models;

namespace SportsWebApp.Controllers
{
    [Authorize(Roles = "Association Manager")]
    public class AssociationManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssociationManagersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssociationManagers
        public IActionResult Index()
        {
            return View();
        }

        // GET: AssociationManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AssociationManagers == null)
            {
                return NotFound();
            }

            var associationManager = await _context.AssociationManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (associationManager == null)
            {
                return NotFound();
            }

            return View(associationManager);
        }

        // GET: AssociationManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssociationManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AssociationManager associationManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(associationManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(associationManager);
        }

        // GET: AssociationManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AssociationManagers == null)
            {
                return NotFound();
            }

            var associationManager = await _context.AssociationManagers.FindAsync(id);
            if (associationManager == null)
            {
                return NotFound();
            }
            return View(associationManager);
        }

        // POST: AssociationManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AssociationManager associationManager)
        {
            if (id != associationManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(associationManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociationManagerExists(associationManager.Id))
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
            return View(associationManager);
        }

        // GET: AssociationManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AssociationManagers == null)
            {
                return NotFound();
            }

            var associationManager = await _context.AssociationManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (associationManager == null)
            {
                return NotFound();
            }

            return View(associationManager);
        }

        // POST: AssociationManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AssociationManagers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AssociationManagers'  is null.");
            }
            var associationManager = await _context.AssociationManagers.FindAsync(id);
            if (associationManager != null)
            {
                _context.AssociationManagers.Remove(associationManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssociationManagerExists(int id)
        {
          return (_context.AssociationManagers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
