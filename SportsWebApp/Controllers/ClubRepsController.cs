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
    public class ClubRepsController : Controller
    {
        private readonly SportsWebAppContext _context;

        public ClubRepsController(SportsWebAppContext context)
        {
            _context = context;
        }

        // GET: ClubReps
        public async Task<IActionResult> Index()
        {
              return _context.ClubRep != null ? 
                          View(await _context.ClubRep.ToListAsync()) :
                          Problem("Entity set 'SportsWebAppContext.ClubRep'  is null.");
        }

        // GET: ClubReps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClubRep == null)
            {
                return NotFound();
            }

            var clubRep = await _context.ClubRep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clubRep == null)
            {
                return NotFound();
            }

            return View(clubRep);
        }

        // GET: ClubReps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClubReps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] ClubRep clubRep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubRep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clubRep);
        }

        // GET: ClubReps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClubRep == null)
            {
                return NotFound();
            }

            var clubRep = await _context.ClubRep.FindAsync(id);
            if (clubRep == null)
            {
                return NotFound();
            }
            return View(clubRep);
        }

        // POST: ClubReps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] ClubRep clubRep)
        {
            if (id != clubRep.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubRep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubRepExists(clubRep.Id))
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
            return View(clubRep);
        }

        // GET: ClubReps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClubRep == null)
            {
                return NotFound();
            }

            var clubRep = await _context.ClubRep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clubRep == null)
            {
                return NotFound();
            }

            return View(clubRep);
        }

        // POST: ClubReps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClubRep == null)
            {
                return Problem("Entity set 'SportsWebAppContext.ClubRep'  is null.");
            }
            var clubRep = await _context.ClubRep.FindAsync(id);
            if (clubRep != null)
            {
                _context.ClubRep.Remove(clubRep);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubRepExists(int id)
        {
          return (_context.ClubRep?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
