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
    public class LjcCoursesController : Controller
    {
        private readonly Project5Context _context;

        public LjcCoursesController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcCourses
        public async Task<IActionResult> Index()
        {
              return _context.LjcCourses != null ? 
                          View(await _context.LjcCourses.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcCourses'  is null.");
        }

        // GET: LjcCourses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.LjcCourses == null)
            {
                return NotFound();
            }

            var ljcCourse = await _context.LjcCourses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (ljcCourse == null)
            {
                return NotFound();
            }

            return View(ljcCourse);
        }

        // GET: LjcCourses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,Description,Credits")] LjcCourse ljcCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcCourse);
        }

        // GET: LjcCourses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.LjcCourses == null)
            {
                return NotFound();
            }

            var ljcCourse = await _context.LjcCourses.FindAsync(id);
            if (ljcCourse == null)
            {
                return NotFound();
            }
            return View(ljcCourse);
        }

        // POST: LjcCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseId,Title,Description,Credits")] LjcCourse ljcCourse)
        {
            if (id != ljcCourse.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcCourseExists(ljcCourse.CourseId))
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
            return View(ljcCourse);
        }

        // GET: LjcCourses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.LjcCourses == null)
            {
                return NotFound();
            }

            var ljcCourse = await _context.LjcCourses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (ljcCourse == null)
            {
                return NotFound();
            }

            return View(ljcCourse);
        }

        // POST: LjcCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.LjcCourses == null)
            {
                return Problem("Entity set 'Project5Context.LjcCourses'  is null.");
            }
            var ljcCourse = await _context.LjcCourses.FindAsync(id);
            if (ljcCourse != null)
            {
                _context.LjcCourses.Remove(ljcCourse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcCourseExists(string id)
        {
          return (_context.LjcCourses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
