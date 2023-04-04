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
    public class LjcPlannedMajorsController : Controller
    {
        private readonly Project5Context _context;

        public LjcPlannedMajorsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcPlannedMajors
        public async Task<IActionResult> Index()
        {
              return _context.LjcPlannedMajors != null ? 
                          View(await _context.LjcPlannedMajors.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcPlannedMajors'  is null.");
        }

        // GET: LjcPlannedMajors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcPlannedMajors == null)
            {
                return NotFound();
            }

            var ljcPlannedMajor = await _context.LjcPlannedMajors
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlannedMajor == null)
            {
                return NotFound();
            }

            return View(ljcPlannedMajor);
        }

        // GET: LjcPlannedMajors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcPlannedMajors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,MajorId")] LjcPlannedMajor ljcPlannedMajor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcPlannedMajor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcPlannedMajor);
        }

        // GET: LjcPlannedMajors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcPlannedMajors == null)
            {
                return NotFound();
            }

            var ljcPlannedMajor = await _context.LjcPlannedMajors.FindAsync(id);
            if (ljcPlannedMajor == null)
            {
                return NotFound();
            }
            return View(ljcPlannedMajor);
        }

        // POST: LjcPlannedMajors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,MajorId")] LjcPlannedMajor ljcPlannedMajor)
        {
            if (id != ljcPlannedMajor.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcPlannedMajor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcPlannedMajorExists(ljcPlannedMajor.PlanId))
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
            return View(ljcPlannedMajor);
        }

        // GET: LjcPlannedMajors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcPlannedMajors == null)
            {
                return NotFound();
            }

            var ljcPlannedMajor = await _context.LjcPlannedMajors
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlannedMajor == null)
            {
                return NotFound();
            }

            return View(ljcPlannedMajor);
        }

        // POST: LjcPlannedMajors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcPlannedMajors == null)
            {
                return Problem("Entity set 'Project5Context.LjcPlannedMajors'  is null.");
            }
            var ljcPlannedMajor = await _context.LjcPlannedMajors.FindAsync(id);
            if (ljcPlannedMajor != null)
            {
                _context.LjcPlannedMajors.Remove(ljcPlannedMajor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcPlannedMajorExists(int id)
        {
          return (_context.LjcPlannedMajors?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
