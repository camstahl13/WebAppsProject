using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ljcProject5.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ljcProject5.Controllers
{
    [Authorize(Roles = "admin")]
    public class LjcMinorRequirementsController : Controller
    {
        private readonly Project5Context _context;

        public LjcMinorRequirementsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcMinorRequirements
        public async Task<IActionResult> Index()
        {
              return _context.LjcMinorRequirements != null ? 
                          View(await _context.LjcMinorRequirements.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcMinorRequirements'  is null.");
        }

        // GET: LjcMinorRequirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcMinorRequirements == null)
            {
                return NotFound();
            }

            var ljcMinorRequirement = await _context.LjcMinorRequirements
                .FirstOrDefaultAsync(m => m.MinorId == id);
            if (ljcMinorRequirement == null)
            {
                return NotFound();
            }

            return View(ljcMinorRequirement);
        }

        // GET: LjcMinorRequirements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcMinorRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MinorId,CatalogYear,CourseId")] LjcMinorRequirement ljcMinorRequirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcMinorRequirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcMinorRequirement);
        }

        // GET: LjcMinorRequirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcMinorRequirements == null)
            {
                return NotFound();
            }

            var ljcMinorRequirement = await _context.LjcMinorRequirements.FindAsync(id);
            if (ljcMinorRequirement == null)
            {
                return NotFound();
            }
            return View(ljcMinorRequirement);
        }

        // POST: LjcMinorRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MinorId,CatalogYear,CourseId")] LjcMinorRequirement ljcMinorRequirement)
        {
            if (id != ljcMinorRequirement.MinorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcMinorRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcMinorRequirementExists(ljcMinorRequirement.MinorId))
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
            return View(ljcMinorRequirement);
        }

        // GET: LjcMinorRequirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcMinorRequirements == null)
            {
                return NotFound();
            }

            var ljcMinorRequirement = await _context.LjcMinorRequirements
                .FirstOrDefaultAsync(m => m.MinorId == id);
            if (ljcMinorRequirement == null)
            {
                return NotFound();
            }

            return View(ljcMinorRequirement);
        }

        // POST: LjcMinorRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcMinorRequirements == null)
            {
                return Problem("Entity set 'Project5Context.LjcMinorRequirements'  is null.");
            }
            var ljcMinorRequirement = await _context.LjcMinorRequirements.FindAsync(id);
            if (ljcMinorRequirement != null)
            {
                _context.LjcMinorRequirements.Remove(ljcMinorRequirement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcMinorRequirementExists(int id)
        {
          return (_context.LjcMinorRequirements?.Any(e => e.MinorId == id)).GetValueOrDefault();
        }
    }
}
