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
    public class LjcAdvisesController : Controller
    {
        private readonly Project5Context _context;

        public LjcAdvisesController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcAdvises
        public async Task<IActionResult> Index()
        {
              return _context.LjcAdvises != null ? 
                          View(await _context.LjcAdvises.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcAdvises'  is null.");
        }

        // GET: LjcAdvises/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.LjcAdvises == null)
            {
                return NotFound();
            }

            var ljcAdvise = await _context.LjcAdvises
                .FirstOrDefaultAsync(m => m.Advisee == id);
            if (ljcAdvise == null)
            {
                return NotFound();
            }

            return View(ljcAdvise);
        }

        // GET: LjcAdvises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcAdvises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Advisor,Advisee")] LjcAdvise ljcAdvise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcAdvise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcAdvise);
        }

        // GET: LjcAdvises/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.LjcAdvises == null)
            {
                return NotFound();
            }

            var ljcAdvise = await _context.LjcAdvises.FindAsync(id);
            if (ljcAdvise == null)
            {
                return NotFound();
            }
            return View(ljcAdvise);
        }

        // POST: LjcAdvises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Advisor,Advisee")] LjcAdvise ljcAdvise)
        {
            if (id != ljcAdvise.Advisee)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcAdvise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcAdviseExists(ljcAdvise.Advisee))
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
            return View(ljcAdvise);
        }

        // GET: LjcAdvises/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.LjcAdvises == null)
            {
                return NotFound();
            }

            var ljcAdvise = await _context.LjcAdvises
                .FirstOrDefaultAsync(m => m.Advisee == id);
            if (ljcAdvise == null)
            {
                return NotFound();
            }

            return View(ljcAdvise);
        }

        // POST: LjcAdvises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.LjcAdvises == null)
            {
                return Problem("Entity set 'Project5Context.LjcAdvises'  is null.");
            }
            var ljcAdvise = await _context.LjcAdvises.FindAsync(id);
            if (ljcAdvise != null)
            {
                _context.LjcAdvises.Remove(ljcAdvise);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcAdviseExists(string id)
        {
          return (_context.LjcAdvises?.Any(e => e.Advisee == id)).GetValueOrDefault();
        }
    }
}
