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
    public class LjcPlannedCoursesController : Controller
    {
        private readonly Project5Context _context;

        public LjcPlannedCoursesController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcPlannedCourses
        public async Task<IActionResult> Index()
        {
              return _context.LjcPlannedCourses != null ? 
                          View(await _context.LjcPlannedCourses.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcPlannedCourses'  is null.");
        }

        // GET: LjcPlannedCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LjcPlannedCourses == null)
            {
                return NotFound();
            }

            var ljcPlannedCourse = await _context.LjcPlannedCourses
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlannedCourse == null)
            {
                return NotFound();
            }

            return View(ljcPlannedCourse);
        }

        // GET: LjcPlannedCourses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcPlannedCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,CourseId,Year,Term")] LjcPlannedCourse ljcPlannedCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcPlannedCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcPlannedCourse);
        }

        // GET: LjcPlannedCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LjcPlannedCourses == null)
            {
                return NotFound();
            }

            var ljcPlannedCourse = await _context.LjcPlannedCourses.FindAsync(id);
            if (ljcPlannedCourse == null)
            {
                return NotFound();
            }
            return View(ljcPlannedCourse);
        }

        // POST: LjcPlannedCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,CourseId,Year,Term")] LjcPlannedCourse ljcPlannedCourse)
        {
            if (id != ljcPlannedCourse.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcPlannedCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcPlannedCourseExists(ljcPlannedCourse.PlanId))
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
            return View(ljcPlannedCourse);
        }

        // GET: LjcPlannedCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LjcPlannedCourses == null)
            {
                return NotFound();
            }

            var ljcPlannedCourse = await _context.LjcPlannedCourses
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlannedCourse == null)
            {
                return NotFound();
            }

            return View(ljcPlannedCourse);
        }

        // POST: LjcPlannedCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LjcPlannedCourses == null)
            {
                return Problem("Entity set 'Project5Context.LjcPlannedCourses'  is null.");
            }
            var ljcPlannedCourse = await _context.LjcPlannedCourses.FindAsync(id);
            if (ljcPlannedCourse != null)
            {
                _context.LjcPlannedCourses.Remove(ljcPlannedCourse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcPlannedCourseExists(int id)
        {
          return (_context.LjcPlannedCourses?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
