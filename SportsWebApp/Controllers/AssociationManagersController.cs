using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Data;
using SportsWebApp.Models;

namespace SportsWebApp.Controllers
{
    public class AssociationManagersController : Controller
    {
        private readonly SportsWebAppContext _context;

        public AssociationManagersController(SportsWebAppContext context)
        {
            _context = context;
        }

        // GET: AssociationManagers
        public async Task<IActionResult> Index()
        {
              return _context.AssociationManager != null ? 
                          View(await _context.AssociationManager.ToListAsync()) :
                          Problem("Entity set 'SportsWebAppContext.AssociationManager'  is null.");
        }

        // GET: AssociationManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AssociationManager == null)
            {
                return NotFound();
            }

            var associationManager = await _context.AssociationManager
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
        public async Task<IActionResult> Create([Bind("Id")] AssociationManager associationManager)
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
            if (id == null || _context.AssociationManager == null)
            {
                return NotFound();
            }

            var associationManager = await _context.AssociationManager.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id")] AssociationManager associationManager)
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
            if (id == null || _context.AssociationManager == null)
            {
                return NotFound();
            }

            var associationManager = await _context.AssociationManager
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
            if (_context.AssociationManager == null)
            {
                return Problem("Entity set 'SportsWebAppContext.AssociationManager'  is null.");
            }
            var associationManager = await _context.AssociationManager.FindAsync(id);
            if (associationManager != null)
            {
                _context.AssociationManager.Remove(associationManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssociationManagerExists(int id)
        {
          return (_context.AssociationManager?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
