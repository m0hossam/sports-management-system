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
    [Authorize(Roles = "Club Representative")]
    public class ClubRepresentativesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClubRepresentativesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClubRepresentatives
        public IActionResult Index()
        {
            return View();
        }

        // GET: ClubRepresentatives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClubRepresentatives == null)
            {
                return NotFound();
            }

            var clubRepresentative = await _context.ClubRepresentatives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clubRepresentative == null)
            {
                return NotFound();
            }

            return View(clubRepresentative);
        }

        // GET: ClubRepresentatives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClubRepresentatives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ClubRepresentative clubRepresentative)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubRepresentative);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clubRepresentative);
        }

        // GET: ClubRepresentatives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClubRepresentatives == null)
            {
                return NotFound();
            }

            var clubRepresentative = await _context.ClubRepresentatives.FindAsync(id);
            if (clubRepresentative == null)
            {
                return NotFound();
            }
            return View(clubRepresentative);
        }

        // POST: ClubRepresentatives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ClubRepresentative clubRepresentative)
        {
            if (id != clubRepresentative.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubRepresentative);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubRepresentativeExists(clubRepresentative.Id))
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
            return View(clubRepresentative);
        }

        // GET: ClubRepresentatives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClubRepresentatives == null)
            {
                return NotFound();
            }

            var clubRepresentative = await _context.ClubRepresentatives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clubRepresentative == null)
            {
                return NotFound();
            }

            return View(clubRepresentative);
        }

        // POST: ClubRepresentatives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClubRepresentatives == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ClubRepresentatives'  is null.");
            }
            var clubRepresentative = await _context.ClubRepresentatives.FindAsync(id);
            if (clubRepresentative != null)
            {
                _context.ClubRepresentatives.Remove(clubRepresentative);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubRepresentativeExists(int id)
        {
          return (_context.ClubRepresentatives?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
