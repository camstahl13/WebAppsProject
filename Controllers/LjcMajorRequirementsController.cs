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
    public class LjcMajorRequirementsController : Controller
    {
        private readonly Project5Context _context;

        public LjcMajorRequirementsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcMajorRequirements
        public async Task<IActionResult> Index()
        {
              return _context.LjcMajorRequirements != null ? 
                          View(await _context.LjcMajorRequirements.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcMajorRequirements'  is null.");
        }

        // GET: LjcMajorRequirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcMajorRequirements == null)
            {
                return NotFound();
            }

            var ljcMajorRequirement = await _context.LjcMajorRequirements
                .FirstOrDefaultAsync(m => m.MajorId == id);
            if (ljcMajorRequirement == null)
            {
                return NotFound();
            }

            return View(ljcMajorRequirement);
        }

        // GET: LjcMajorRequirements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcMajorRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MajorId,CatalogYear,CourseId,Category")] LjcMajorRequirement ljcMajorRequirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcMajorRequirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcMajorRequirement);
        }

        // GET: LjcMajorRequirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcMajorRequirements == null)
            {
                return NotFound();
            }

            var ljcMajorRequirement = await _context.LjcMajorRequirements.FindAsync(id);
            if (ljcMajorRequirement == null)
            {
                return NotFound();
            }
            return View(ljcMajorRequirement);
        }

        // POST: LjcMajorRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MajorId,CatalogYear,CourseId,Category")] LjcMajorRequirement ljcMajorRequirement)
        {
            if (id != ljcMajorRequirement.MajorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcMajorRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcMajorRequirementExists(ljcMajorRequirement.MajorId))
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
            return View(ljcMajorRequirement);
        }

        // GET: LjcMajorRequirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcMajorRequirements == null)
            {
                return NotFound();
            }

            var ljcMajorRequirement = await _context.LjcMajorRequirements
                .FirstOrDefaultAsync(m => m.MajorId == id);
            if (ljcMajorRequirement == null)
            {
                return NotFound();
            }

            return View(ljcMajorRequirement);
        }

        // POST: LjcMajorRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcMajorRequirements == null)
            {
                return Problem("Entity set 'Project5Context.LjcMajorRequirements'  is null.");
            }
            var ljcMajorRequirement = await _context.LjcMajorRequirements.FindAsync(id);
            if (ljcMajorRequirement != null)
            {
                _context.LjcMajorRequirements.Remove(ljcMajorRequirement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcMajorRequirementExists(int id)
        {
          return (_context.LjcMajorRequirements?.Any(e => e.MajorId == id)).GetValueOrDefault();
        }
    }
}
