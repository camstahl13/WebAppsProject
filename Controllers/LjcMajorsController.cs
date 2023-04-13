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
    public class LjcMajorsController : Controller
    {
        private readonly Project5Context _context;

        public LjcMajorsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcMajors
        public async Task<IActionResult> Index()
        {
              return _context.LjcMajors != null ? 
                          View(await _context.LjcMajors.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcMajors'  is null.");
        }

        // GET: LjcMajors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcMajors == null)
            {
                return NotFound();
            }

            var ljcMajor = await _context.LjcMajors
                .FirstOrDefaultAsync(m => m.MajorId == id);
            if (ljcMajor == null)
            {
                return NotFound();
            }

            return View(ljcMajor);
        }

        // GET: LjcMajors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcMajors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MajorId,Major")] LjcMajor ljcMajor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcMajor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcMajor);
        }

        // GET: LjcMajors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcMajors == null)
            {
                return NotFound();
            }

            var ljcMajor = await _context.LjcMajors.FindAsync(id);
            if (ljcMajor == null)
            {
                return NotFound();
            }
            return View(ljcMajor);
        }

        // POST: LjcMajors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MajorId,Major")] LjcMajor ljcMajor)
        {
            if (id != ljcMajor.MajorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcMajor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcMajorExists(ljcMajor.MajorId))
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
            return View(ljcMajor);
        }

        // GET: LjcMajors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcMajors == null)
            {
                return NotFound();
            }

            var ljcMajor = await _context.LjcMajors
                .FirstOrDefaultAsync(m => m.MajorId == id);
            if (ljcMajor == null)
            {
                return NotFound();
            }

            return View(ljcMajor);
        }

        // POST: LjcMajors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcMajors == null)
            {
                return Problem("Entity set 'Project5Context.LjcMajors'  is null.");
            }
            var ljcMajor = await _context.LjcMajors.FindAsync(id);
            if (ljcMajor != null)
            {
                _context.LjcMajors.Remove(ljcMajor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcMajorExists(int id)
        {
          return (_context.LjcMajors?.Any(e => e.MajorId == id)).GetValueOrDefault();
        }
    }
}
