using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ljcProject5.Models;

namespace ljcProject5.Controllers
{
    public class LjcPlannedMinorsController : Controller
    {
        private readonly Project5Context _context;

        public LjcPlannedMinorsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcPlannedMinors
        public async Task<IActionResult> Index()
        {
              return _context.LjcPlannedMinors != null ? 
                          View(await _context.LjcPlannedMinors.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcPlannedMinors'  is null.");
        }

        // GET: LjcPlannedMinors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcPlannedMinors == null)
            {
                return NotFound();
            }

            var ljcPlannedMinor = await _context.LjcPlannedMinors
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlannedMinor == null)
            {
                return NotFound();
            }

            return View(ljcPlannedMinor);
        }

        // GET: LjcPlannedMinors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcPlannedMinors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,MinorId")] LjcPlannedMinor ljcPlannedMinor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcPlannedMinor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcPlannedMinor);
        }

        // GET: LjcPlannedMinors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcPlannedMinors == null)
            {
                return NotFound();
            }

            var ljcPlannedMinor = await _context.LjcPlannedMinors.FindAsync(id);
            if (ljcPlannedMinor == null)
            {
                return NotFound();
            }
            return View(ljcPlannedMinor);
        }

        // POST: LjcPlannedMinors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,MinorId")] LjcPlannedMinor ljcPlannedMinor)
        {
            if (id != ljcPlannedMinor.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcPlannedMinor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcPlannedMinorExists(ljcPlannedMinor.PlanId))
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
            return View(ljcPlannedMinor);
        }

        // GET: LjcPlannedMinors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcPlannedMinors == null)
            {
                return NotFound();
            }

            var ljcPlannedMinor = await _context.LjcPlannedMinors
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlannedMinor == null)
            {
                return NotFound();
            }

            return View(ljcPlannedMinor);
        }

        // POST: LjcPlannedMinors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcPlannedMinors == null)
            {
                return Problem("Entity set 'Project5Context.LjcPlannedMinors'  is null.");
            }
            var ljcPlannedMinor = await _context.LjcPlannedMinors.FindAsync(id);
            if (ljcPlannedMinor != null)
            {
                _context.LjcPlannedMinors.Remove(ljcPlannedMinor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcPlannedMinorExists(int id)
        {
          return (_context.LjcPlannedMinors?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
