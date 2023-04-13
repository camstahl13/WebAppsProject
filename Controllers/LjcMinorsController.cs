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
    public class LjcMinorsController : Controller
    {
        private readonly Project5Context _context;

        public LjcMinorsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcMinors
        public async Task<IActionResult> Index()
        {
              return _context.LjcMinors != null ? 
                          View(await _context.LjcMinors.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcMinors'  is null.");
        }

        // GET: LjcMinors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcMinors == null)
            {
                return NotFound();
            }

            var ljcMinor = await _context.LjcMinors
                .FirstOrDefaultAsync(m => m.MinorId == id);
            if (ljcMinor == null)
            {
                return NotFound();
            }

            return View(ljcMinor);
        }

        // GET: LjcMinors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcMinors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MinorId,Minor")] LjcMinor ljcMinor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcMinor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcMinor);
        }

        // GET: LjcMinors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcMinors == null)
            {
                return NotFound();
            }

            var ljcMinor = await _context.LjcMinors.FindAsync(id);
            if (ljcMinor == null)
            {
                return NotFound();
            }
            return View(ljcMinor);
        }

        // POST: LjcMinors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MinorId,Minor")] LjcMinor ljcMinor)
        {
            if (id != ljcMinor.MinorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcMinor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcMinorExists(ljcMinor.MinorId))
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
            return View(ljcMinor);
        }

        // GET: LjcMinors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcMinors == null)
            {
                return NotFound();
            }

            var ljcMinor = await _context.LjcMinors
                .FirstOrDefaultAsync(m => m.MinorId == id);
            if (ljcMinor == null)
            {
                return NotFound();
            }

            return View(ljcMinor);
        }

        // POST: LjcMinors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcMinors == null)
            {
                return Problem("Entity set 'Project5Context.LjcMinors'  is null.");
            }
            var ljcMinor = await _context.LjcMinors.FindAsync(id);
            if (ljcMinor != null)
            {
                _context.LjcMinors.Remove(ljcMinor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcMinorExists(int id)
        {
          return (_context.LjcMinors?.Any(e => e.MinorId == id)).GetValueOrDefault();
        }
    }
}
