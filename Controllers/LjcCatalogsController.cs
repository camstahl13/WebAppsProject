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
    public class LjcCatalogsController : Controller
    {
        private readonly Project5Context _context;

        public LjcCatalogsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcCatalogs
        public async Task<IActionResult> Index()
        {
              return _context.LjcCatalogs != null ? 
                          View(await _context.LjcCatalogs.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcCatalogs'  is null.");
        }

        // GET: LjcCatalogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcCatalogs == null)
            {
                return NotFound();
            }

            var ljcCatalog = await _context.LjcCatalogs
                .FirstOrDefaultAsync(m => m.CatalogYear == id);
            if (ljcCatalog == null)
            {
                return NotFound();
            }

            return View(ljcCatalog);
        }

        // GET: LjcCatalogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcCatalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogYear,CourseId")] LjcCatalog ljcCatalog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcCatalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcCatalog);
        }

        // GET: LjcCatalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcCatalogs == null)
            {
                return NotFound();
            }

            var ljcCatalog = await _context.LjcCatalogs.FindAsync(id);
            if (ljcCatalog == null)
            {
                return NotFound();
            }
            return View(ljcCatalog);
        }

        // POST: LjcCatalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatalogYear,CourseId")] LjcCatalog ljcCatalog)
        {
            if (id != ljcCatalog.CatalogYear)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcCatalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcCatalogExists(ljcCatalog.CatalogYear))
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
            return View(ljcCatalog);
        }

        // GET: LjcCatalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcCatalogs == null)
            {
                return NotFound();
            }

            var ljcCatalog = await _context.LjcCatalogs
                .FirstOrDefaultAsync(m => m.CatalogYear == id);
            if (ljcCatalog == null)
            {
                return NotFound();
            }

            return View(ljcCatalog);
        }

        // POST: LjcCatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcCatalogs == null)
            {
                return Problem("Entity set 'Project5Context.LjcCatalogs'  is null.");
            }
            var ljcCatalog = await _context.LjcCatalogs.FindAsync(id);
            if (ljcCatalog != null)
            {
                _context.LjcCatalogs.Remove(ljcCatalog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcCatalogExists(int id)
        {
          return (_context.LjcCatalogs?.Any(e => e.CatalogYear == id)).GetValueOrDefault();
        }
    }
}
