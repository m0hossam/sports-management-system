using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestApp.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class DummiesController : Controller
    {
        private readonly TestAppContext _context;

        public DummiesController(TestAppContext context)
        {
            _context = context;
        }

        // GET: Dummies
        public async Task<IActionResult> Index()
        {
              return _context.Dummy != null ? 
                          View(await _context.Dummy.ToListAsync()) :
                          Problem("Entity set 'TestAppContext.Dummy'  is null.");
        }

        // GET: Dummies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dummy == null)
            {
                return NotFound();
            }

            var dummy = await _context.Dummy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dummy == null)
            {
                return NotFound();
            }

            return View(dummy);
        }

        // GET: Dummies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dummies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Dummy dummy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dummy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dummy);
        }

        // GET: Dummies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dummy == null)
            {
                return NotFound();
            }

            var dummy = await _context.Dummy.FindAsync(id);
            if (dummy == null)
            {
                return NotFound();
            }
            return View(dummy);
        }

        // POST: Dummies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Dummy dummy)
        {
            if (id != dummy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dummy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DummyExists(dummy.Id))
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
            return View(dummy);
        }

        // GET: Dummies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dummy == null)
            {
                return NotFound();
            }

            var dummy = await _context.Dummy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dummy == null)
            {
                return NotFound();
            }

            return View(dummy);
        }

        // POST: Dummies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dummy == null)
            {
                return Problem("Entity set 'TestAppContext.Dummy'  is null.");
            }
            var dummy = await _context.Dummy.FindAsync(id);
            if (dummy != null)
            {
                _context.Dummy.Remove(dummy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DummyExists(int id)
        {
          return (_context.Dummy?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
