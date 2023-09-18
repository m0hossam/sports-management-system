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
    [Authorize(Roles = "Stadium Manager")]
    public class StadiumManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StadiumManagersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StadiumManagers
        public IActionResult Index()
        {
            return View();
        }

        // GET: StadiumManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StadiumManagers == null)
            {
                return NotFound();
            }

            var stadiumManager = await _context.StadiumManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadiumManager == null)
            {
                return NotFound();
            }

            return View(stadiumManager);
        }

        // GET: StadiumManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StadiumManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] StadiumManager stadiumManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stadiumManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stadiumManager);
        }

        // GET: StadiumManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StadiumManagers == null)
            {
                return NotFound();
            }

            var stadiumManager = await _context.StadiumManagers.FindAsync(id);
            if (stadiumManager == null)
            {
                return NotFound();
            }
            return View(stadiumManager);
        }

        // POST: StadiumManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] StadiumManager stadiumManager)
        {
            if (id != stadiumManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadiumManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadiumManagerExists(stadiumManager.Id))
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
            return View(stadiumManager);
        }

        // GET: StadiumManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StadiumManagers == null)
            {
                return NotFound();
            }

            var stadiumManager = await _context.StadiumManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadiumManager == null)
            {
                return NotFound();
            }

            return View(stadiumManager);
        }

        // POST: StadiumManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StadiumManagers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StadiumManagers'  is null.");
            }
            var stadiumManager = await _context.StadiumManagers.FindAsync(id);
            if (stadiumManager != null)
            {
                _context.StadiumManagers.Remove(stadiumManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadiumManagerExists(int id)
        {
          return (_context.StadiumManagers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
