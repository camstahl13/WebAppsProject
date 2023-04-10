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
    [Route("Admin/[controller]/[action]")]
    public class RolesController : Controller
    {
        private readonly Project5Context _context;

        public RolesController(Project5Context context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
              return _context.Aspnetroles != null ? 
                          View(await _context.Aspnetroles.ToListAsync()) :
                          Problem("Entity set 'Project5Context.Aspnetroles'  is null.");
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Aspnetroles == null)
            {
                return NotFound();
            }

            var aspnetrole = await _context.Aspnetroles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspnetrole == null)
            {
                return NotFound();
            }

            return View(aspnetrole);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] Aspnetrole aspnetrole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspnetrole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspnetrole);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Aspnetroles == null)
            {
                return NotFound();
            }

            var aspnetrole = await _context.Aspnetroles.FindAsync(id);
            if (aspnetrole == null)
            {
                return NotFound();
            }
            return View(aspnetrole);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] Aspnetrole aspnetrole)
        {
            if (id != aspnetrole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspnetrole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspnetroleExists(aspnetrole.Id))
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
            return View(aspnetrole);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Aspnetroles == null)
            {
                return NotFound();
            }

            var aspnetrole = await _context.Aspnetroles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspnetrole == null)
            {
                return NotFound();
            }

            return View(aspnetrole);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Aspnetroles == null)
            {
                return Problem("Entity set 'Project5Context.Aspnetroles'  is null.");
            }
            var aspnetrole = await _context.Aspnetroles.FindAsync(id);
            if (aspnetrole != null)
            {
                _context.Aspnetroles.Remove(aspnetrole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspnetroleExists(string id)
        {
          return (_context.Aspnetroles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
