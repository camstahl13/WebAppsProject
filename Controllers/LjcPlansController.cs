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
    public class LjcPlansController : Controller
    {
        private readonly Project5Context _context;

        public LjcPlansController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcPlans
        public async Task<IActionResult> Index()
        {
              return _context.LjcPlans != null ? 
                          View(await _context.LjcPlans.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcPlans'  is null.");
        }

        // GET: LjcPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcPlans == null)
            {
                return NotFound();
            }

            var ljcPlan = await _context.LjcPlans
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlan == null)
            {
                return NotFound();
            }

            return View(ljcPlan);
        }

        // GET: LjcPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,Planname,Username,CatalogYear,Default")] LjcPlan ljcPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcPlan);
        }

        // GET: LjcPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcPlans == null)
            {
                return NotFound();
            }

            var ljcPlan = await _context.LjcPlans.FindAsync(id);
            if (ljcPlan == null)
            {
                return NotFound();
            }
            return View(ljcPlan);
        }

        // POST: LjcPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,Planname,Username,CatalogYear,Default")] LjcPlan ljcPlan)
        {
            if (id != ljcPlan.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcPlanExists(ljcPlan.PlanId))
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
            return View(ljcPlan);
        }

        // GET: LjcPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcPlans == null)
            {
                return NotFound();
            }

            var ljcPlan = await _context.LjcPlans
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlan == null)
            {
                return NotFound();
            }

            return View(ljcPlan);
        }

        // POST: LjcPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcPlans == null)
            {
                return Problem("Entity set 'Project5Context.LjcPlans'  is null.");
            }
            var ljcPlan = await _context.LjcPlans.FindAsync(id);
            if (ljcPlan != null)
            {
                _context.LjcPlans.Remove(ljcPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcPlanExists(int id)
        {
          return (_context.LjcPlans?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
