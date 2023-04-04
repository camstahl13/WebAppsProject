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
    public class LjcUsersController : Controller
    {
        private readonly Project5Context _context;

        public LjcUsersController(Project5Context context)
        {
            _context = context;
        }

        // GET: LjcUsers
        public async Task<IActionResult> Index()
        {
              return _context.LjcUsers != null ? 
                          View(await _context.LjcUsers.ToListAsync()) :
                          Problem("Entity set 'Project5Context.LjcUsers'  is null.");
        }

        // GET: LjcUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.LjcUsers == null)
            {
                return NotFound();
            }

            var ljcUser = await _context.LjcUsers
                .FirstOrDefaultAsync(m => m.Username == id);
            if (ljcUser == null)
            {
                return NotFound();
            }

            return View(ljcUser);
        }

        // GET: LjcUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LjcUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,FirstName,LastName")] LjcUser ljcUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ljcUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ljcUser);
        }

        // GET: LjcUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.LjcUsers == null)
            {
                return NotFound();
            }

            var ljcUser = await _context.LjcUsers.FindAsync(id);
            if (ljcUser == null)
            {
                return NotFound();
            }
            return View(ljcUser);
        }

        // POST: LjcUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,FirstName,LastName")] LjcUser ljcUser)
        {
            if (id != ljcUser.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ljcUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LjcUserExists(ljcUser.Username))
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
            return View(ljcUser);
        }

        // GET: LjcUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.LjcUsers == null)
            {
                return NotFound();
            }

            var ljcUser = await _context.LjcUsers
                .FirstOrDefaultAsync(m => m.Username == id);
            if (ljcUser == null)
            {
                return NotFound();
            }

            return View(ljcUser);
        }

        // POST: LjcUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.LjcUsers == null)
            {
                return Problem("Entity set 'Project5Context.LjcUsers'  is null.");
            }
            var ljcUser = await _context.LjcUsers.FindAsync(id);
            if (ljcUser != null)
            {
                _context.LjcUsers.Remove(ljcUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LjcUserExists(string id)
        {
          return (_context.LjcUsers?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
