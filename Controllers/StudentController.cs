using ljcProject5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ljcProject5.Controllers
{
    public class StudentController : Controller
    {
        private readonly Project5Context _context;

        public StudentController(Project5Context context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Ape(string student, int? plan)
        {
            if (student.IsNullOrEmpty() || User.Identity == null)
            {
                return NotFound();
            }
            string? username = User.Identity.Name;

            if (User.IsInRole("faculty"))
            {
                var advisees = (from advises in _context.LjcAdvises
                                where advises.Advisor == username && advises.Advisee == student
                                select advises.Advisor);
                if (!advisees.Any())
                {
                    return Unauthorized();
                }
            } else if (User.IsInRole("student"))
            {
                if (username != student)
                {
                    return Unauthorized();
                }
            }
            HttpContext.Session.SetString("student", student);
            
            if (!plan.HasValue)
            {
                var default_plan = (from p in _context.LjcPlans
                                    where p.Username == student && p.Default == true
                                    select p.PlanId).ToList();
                if (default_plan.Any())
                {
                    HttpContext.Session.SetInt32("plan", default_plan[0]);
                } else
                {
                    HttpContext.Session.SetInt32("plan", -1);
                }
            } else {
                HttpContext.Session.SetInt32("plan", (int)plan);
            }
            return View("~/Views/Student/Index.cshtml");
        }

        [Authorize(Roles = "student")]
        public IActionResult Index()
        {
            return Redirect("/");
        }
    }
}
