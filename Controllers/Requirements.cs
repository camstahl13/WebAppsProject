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
    [Route("Internal/[controller]/[action]")]
    public class RequirementsController : Controller
    {
        private readonly Project5Context _context;

        public RequirementsController(Project5Context context)
        {
            _context = context;
        }

        // GET: Requirements/Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null || _context.LjcPlans == null)
            {
                return NotFound();
            }

            var plans = _context.LjcPlans;
            var users = _context.LjcUsers;
            var results = (from p in plans join u in users
                           on p.Username equals u.Username
                           where p.Username == "campbell"
                           select new { p.Planname }).ToList(); 
            /*var ljcPlan = await _context.LjcPlans
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (ljcPlan == null)
            {
                return NotFound();
            }
            */

            return Json(results);
        }
    }
}
