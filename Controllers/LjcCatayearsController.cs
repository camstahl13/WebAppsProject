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
    public class LjcCatayearsController : Controller
    {
        private readonly Project5Context _context;

        public LjcCatayearsController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcCatayears
        public async Task<IActionResult> Index()
        {
              return _context.LjcCatayears != null ? 
                          View(await _context.LjcCatayears.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcCatayears'  is null.");
        }

        // GET: LjcCatayears/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcCatayears == null)
            {
                return NotFound();
            }

            var ljcCatayear = await _context.LjcCatayears
                .FirstOrDefaultAsync(m => m.CatalogYear == id);
            if (ljcCatayear == null)
            {
                return NotFound();
            }

            return View(ljcCatayear);
        }

        // GET: LjcCatayears/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcCatayears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogYear")] LjcCatayear ljcCatayear)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcCatayear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcCatayear);
        }

        // GET: LjcCatayears/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcCatayears == null)
            {
                return NotFound();
            }

            var ljcCatayear = await _context.LjcCatayears.FindAsync(id);
            if (ljcCatayear == null)
            {
                return NotFound();
            }
            return View(ljcCatayear);
        }

        // POST: LjcCatayears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatalogYear")] LjcCatayear ljcCatayear)
        {
            if (id != ljcCatayear.CatalogYear)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcCatayear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcCatayearExists(ljcCatayear.CatalogYear))
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
            return View(ljcCatayear);
        }

        // GET: LjcCatayears/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcCatayears == null)
            {
                return NotFound();
            }

            var ljcCatayear = await _context.LjcCatayears
                .FirstOrDefaultAsync(m => m.CatalogYear == id);
            if (ljcCatayear == null)
            {
                return NotFound();
            }

            return View(ljcCatayear);
        }

        // POST: LjcCatayears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcCatayears == null)
            {
                return Problem("Entity set 'Project5Context.LjcCatayears'  is null.");
            }
            var ljcCatayear = await _context.LjcCatayears.FindAsync(id);
            if (ljcCatayear != null)
            {
                _context.LjcCatayears.Remove(ljcCatayear);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcCatayearExists(int id)
        {
          return (_context.LjcCatayears?.Any(e => e.CatalogYear == id)).GetValueOrDefault();
        }
    }
}
