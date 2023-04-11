using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ljcProject5.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using ljcProject5.Areas.Identity.Data;
using ljcProject5.Controllers;
using System.Runtime.ConstrainedExecution;
using ljcProject5.Migrations;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;

namespace ljcProject5.Controllers
{

    public class Requirements
    {
        public Dictionary<string, Category> categories;
        public Requirements()
        {
            categories = new Dictionary<string, Category>();
        }
        public void addCategory(string cat)
        {
            if (categories.ContainsKey(cat))
                return;

            categories[cat] = new Category();
        }
    }

    public class Category
    {
        public List<string> courses;
        public Category()
        {
            courses = new List<string>();
        }
        public void addCourse(string course)
        {
            courses.Add(course);
        }
    }

    class PlanCourse
    {
        public string id;
	    public int year;
	    public string term;

	    public PlanCourse(string i, int y, string t)
        {
            id = i;
            year = y;
            term = t;
        }
    }

    class CatCourse
    {
        public string id;
        public string name;
        public string description;
        public int credits;

        public CatCourse(string i, string n, string d, int c)
        {
            id = i;
            name = n;
            description = d;
            credits = c;
        }
    }

    class Catalog
    {
        public int year;
        public Dictionary<string, CatCourse> courses;

	    public Catalog(int y)
        {
            year = y;
            courses = new Dictionary<string, CatCourse>();
        }

        public void addCourse(string i, string n, string d, int c)
        {
		        courses[i] = new CatCourse(i, n, d, c);
        }
    }

    class Plan
    {
        public string student;
	    public int id;
	    public string major;
	    public string minor;
        public int catYear;
	    public int currYear;
	    public string currTerm;
	    public Dictionary<string, PlanCourse> courses;

	    public Plan(string s, int i, string maj, string min, int catyear, int curryear, string currterm)
        {
		    student = s;
		    id = i;
		    major = maj;
		    minor = min;
		    catYear = catyear;
		    currYear = curryear;
		    currTerm = currterm;
		    courses = new Dictionary<string, PlanCourse>();
        }

        public void addCourse(string i, int y, string t)
        {
		        courses[i] = new PlanCourse(i, y, t);
        }
    }

    class UserPlans
    {
        public Dictionary<int, UserPlan> plans;
        public UserPlans()
        {
            plans = new Dictionary<int, UserPlan>();
        }
        public void addPlan(int plan_id, UserPlan plan)
        {
            plans[plan_id] = plan;
        }
    }

    class UserPlan
    {
        public string planname;
        public int catalog_year;
        public List<string> majors;
        public List<string> minors;
	    public bool is_default;

        public UserPlan(string pn, int cy, List<string> maj, List<string> min, bool def)
        {
            planname = pn;
            catalog_year = cy;
            majors = maj;
            minors = min;
            is_default = def;
        }
    }

    class Advisorship
    {
        public string advisor;
        public List<string> advisees;

        public Advisorship(string adv)
        {
            advisor = adv;
            advisees = new List<string>();
        }

        public void addAdvisee(string adv)
        {
            advisees.Add(adv);
        }
    }

    [Route("Internal/[controller]/[action]")]
    public class RequirementsController : Controller
    {
        private readonly Project5Context _context;

        public RequirementsController(Project5Context context)
        {
            _context = context;
        }

        [HttpGet]
        public string GetAdvisees()
        {
            if (User.Identity == null || User.Identity.Name == null || !(User.IsInRole("faculty") || User.IsInRole("admin")))
            {
                return "";
            }
            string username = User.Identity.Name;

            var advisorship = new Advisorship(username);
            var advisees = (from advise in _context.LjcAdvises
                            where advise.Advisor == username
                            select advise.Advisee).ToList();
            foreach (var advisee in advisees)
            {
                advisorship.addAdvisee(advisee);
            }
            return JsonConvert.SerializeObject(advisorship);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePlan(string planname)
        {
            string? student = HttpContext.Session.GetString("student");
            var plans = (from plan in _context.LjcPlans
                         where plan.Planname == planname && plan.Username == student
                         select plan.PlanId).ToList();
            if (plans.Any())
            {
                return RedirectToPage("/Student/Ape?student=" + student + "&plan=" + plans.First());
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan(string name, string createMajor, string createMinor, string catayear) {
            string planname = name;
            string major = createMajor;
            string minor = createMinor;
            int year = int.Parse(catayear);
            string? username = HttpContext.Session.GetString("student");
            if (username == null)
            {
                return Unauthorized();
            }

            //make sure no plans associated with that user have the same name and modify if necessary
            while ((from plan in _context.LjcPlans
                    where plan.Username == username && plan.Planname == planname
                    select plan.Planname).ToList().Any())
            {
                planname += "_";
            }

            LjcPlan newPlan = new LjcPlan();
            newPlan.Planname = planname;
            newPlan.Username = username;
            newPlan.CatalogYear = year;
            newPlan.Default = !(from plan in _context.LjcPlans 
                                where plan.Username == username
                                select plan.PlanId).ToList().Any();

            _context.Add(newPlan);
            _context.SaveChanges();

            var plans_ids = (from plan in _context.LjcPlans
                             where plan.Username == username && plan.Planname == planname
                             select plan.PlanId).ToList();
            if (!plans_ids.Any())
            {
                return NotFound();
            }
            var plan_id = plans_ids.First();


            var major_ids = (from maj in _context.LjcMajors
                             where maj.Major == major
                             select maj.MajorId).ToList();
            if (!major_ids.Any()) { return NotFound(); }
            var major_id = major_ids.First();

            var minor_ids = (from min in _context.LjcMinors
                             where min.Minor == minor
                             select min.MinorId).ToList();
            if (!minor_ids.Any()) { return NotFound(); }
            var minor_id = minor_ids.First();

            var newPlannedMajor = new LjcPlannedMajor();
            newPlannedMajor.PlanId = plan_id;
            newPlannedMajor.MajorId = major_id;
            _context.Add(newPlannedMajor);

            var newPlannedMinor = new LjcPlannedMinor();
            newPlannedMinor.PlanId = plan_id;
            newPlannedMinor.MinorId = minor_id;
            _context.Add(newPlannedMinor);

            var genEdsMinor = new LjcPlannedMinor();
            genEdsMinor.PlanId = plan_id;
            genEdsMinor.MinorId = 5;
            _context.LjcPlannedMinors.Add(genEdsMinor);

            _context.SaveChanges();

            var planned_majors = (from maj in _context.LjcPlannedMajors
                                  where maj.PlanId == plan_id
                                  select maj.MajorId).ToList();
            foreach (var planned_major in planned_majors)
            {
                var planned_courses = (from req in _context.LjcMajorRequirements
                                       where req.MajorId == planned_major
                                       select req.CourseId).ToList();
                int termTracker = 0;
                foreach (var course in planned_courses)
                {
                    int yr = year + termTracker / 2;
                    string sem = termTracker % 2 == 0 ? "FA" : "SP";
                    var newPlannedCourse = new LjcPlannedCourse();
                    newPlannedCourse.Year = sem != "FA" ? yr + 1 : yr;
                    newPlannedCourse.Term = sem;
                    newPlannedCourse.PlanId = plan_id;
                    newPlannedCourse.CourseId = course;
                    _context.Add(newPlannedCourse);
                    termTracker++;
                    termTracker = (termTracker) % 8;
                }
            }

            var planned_minors = (from min in _context.LjcPlannedMinors
                                  where min.PlanId == plan_id
                                  select min.MinorId).ToList();
            foreach (var planned_minor in planned_minors)
            {
                var planned_courses = (from req in _context.LjcMinorRequirements
                                       where req.MinorId == planned_minor
                                       select req.CourseId).ToList();
                int termTracker = 0;
                foreach (var course in planned_courses)
                {
                    int yr = year + termTracker / 2;
                    string sem = termTracker % 2 == 0 ? "FA" : "SP";
                    var newPlannedCourse = new LjcPlannedCourse();
                    newPlannedCourse.Year = sem != "FA" ? yr + 1: yr ;
                    newPlannedCourse.Term = sem;
                    newPlannedCourse.PlanId = plan_id;
                    newPlannedCourse.CourseId = course;
                    _context.Add(newPlannedCourse);
                    termTracker++;
                    termTracker = (termTracker) % 8;
                }
            }
            _context.SaveChanges();
            return Redirect("/Student/Ape?student=" + username + "&plan=" + plan_id);
        }

        [HttpGet]
        public string GetPlannable()
        {
            var majors = (from maj in _context.LjcMajors
                          select maj.Major).ToList();
            var minors = (from min in _context.LjcMinors
                          where min.MinorId != 5
                          select min.Minor).ToList();
            var years = (from yr in _context.LjcCatayears
                         select yr.CatalogYear).ToList();
            List<object> res = new List<object>();
            res.Add(majors);
            res.Add(minors);
            res.Add(years);

            return JsonConvert.SerializeObject(res);
        }
            

        [HttpGet]
        public string GetPlans()
        {
            var user_plans = new UserPlans();
            string? username = HttpContext.Session.GetString("student");
            int? plan_id = HttpContext.Session.GetInt32("plan");
            if (username == null || plan_id == null)
            {
                return "Testing";
            }
            var plans = (from plan in _context.LjcPlans
                         where plan.Username == username
                         select new { pid = plan.PlanId, pnm = plan.Planname, pcy = plan.CatalogYear, pdf = plan.Default }).ToList();
            foreach (var plan in plans)
            {
                var majors = (from planmaj in _context.LjcPlannedMajors
                                      join maj in _context.LjcMajors
                                      on planmaj.MajorId equals maj.MajorId
                                      where planmaj.PlanId == plan.pid
                                      select maj.Major).ToList();
                var minors = (from planmin in _context.LjcPlannedMinors
                                      join min in _context.LjcMinors
                                      on planmin.MinorId equals min.MinorId
                                      where planmin.PlanId == plan.pid
                                      select min.Minor).ToList();

                user_plans.addPlan(plan.pid, 
                    new UserPlan(plan.pnm, plan.pcy, majors, minors, plan.pdf));
            }

            return JsonConvert.SerializeObject(user_plans);
        }

        [HttpGet]
        public async Task<string> GetCombined()
        {
            string? username = HttpContext.Session.GetString("student");
            int? plan_id = HttpContext.Session.GetInt32("plan");
            if (plan_id == null || username == null || _context.LjcPlans == null)
            {
                return "";// NotFound();
            }

            if (plan_id == -1)
            {
                return "{\"nodefault\":1}";
            }

            const int CURR_YEAR = 2023;
            const string CURR_TERM = "Spring";

            var cat_year_results = (from p in _context.LjcPlans
                                    where p.PlanId == plan_id
                                    select p.CatalogYear).ToList();

            if (cat_year_results.Count() < 1 || cat_year_results.Count() > 1) {
                return "";
            }

            var catalog_year = cat_year_results[0];
            var catalog = new Catalog(catalog_year);

            var catCourses = (from cat in _context.LjcCatalogs
                              where cat.CatalogYear >= catalog_year
                              select cat.CourseId).ToList();

            foreach (var course_id in catCourses) {
                var courses = (from course in _context.LjcCourses
                               where course.CourseId == course_id
                               select new { course_id = course.CourseId, title = course.Title, desc = course.Description, creds = course.Credits }).ToList();
	            if (courses.Count() == 1)
                {
                    var course = courses[0];
                    catalog.addCourse(course.course_id, course.title, course.desc, course.creds);
                }
            }

            var majors = new List<string>();
            var major_results = (from plan_maj in _context.LjcPlannedMajors
                                 join maj in _context.LjcMajors
                                 on plan_maj.MajorId equals maj.MajorId
                                 where plan_maj.PlanId == plan_id
                                 select maj.Major).ToList();

            foreach (var major in major_results) {
                majors.Add(major);
            }

            var minors = new List<string>();
            var minor_results = (from plan_min in _context.LjcPlannedMinors
                                 join min in _context.LjcMinors
                                 on plan_min.MinorId equals min.MinorId
                                 where plan_min.PlanId == plan_id
                                 select min.Minor).ToList();

            foreach (var minor in minor_results)
            {
                minors.Add(minor);
            }

            var course_results = (from plan_course in _context.LjcPlannedCourses
                                  where plan_course.PlanId == plan_id
                                  select new { course_id = plan_course.CourseId, year = plan_course.Year, term = plan_course.Term }).ToList();

            //UserManager<ljcProject5User> _userManager;
            //var userID = await _userManager.FindByIdAsync()
            var plan = new Plan(username,
                                (int)plan_id,
                                string.Join(", ", majors.ToArray()),
                                string.Join(", ", minors.ToArray()),
                                catalog_year,
                                CURR_YEAR,
                                CURR_TERM);
            
            foreach(var course in course_results) {
	            var term = "Fall";
	            if (course.term == "SP") {
		            term = "Spring";
	            } else if (course.term == "SU") {
		            term = "Summer";
	            }
	            plan.addCourse(course.course_id, course.year, term);
            }

            var combined = new
            {
                plan,
                catalog
            };

            return JsonConvert.SerializeObject(combined);
        }

        // GET: Internal/Requirements/Get
        [HttpGet]
        public async Task</*IActionResult*/string> Get()
        {
            int? plan_id = HttpContext.Session.GetInt32("plan");
            if (plan_id == null || _context.LjcPlans == null)
            {
                return "";// NotFound();
            }

            if (plan_id == -1)
            {
                return "{\"nodefault\":1}";
            }

            var requirements = new Requirements();
            var plans = _context.LjcPlans;

            var majors = (from plan_maj in _context.LjcPlannedMajors join maj in _context.LjcMajors
                           on plan_maj.MajorId equals maj.MajorId
                          where plan_maj.PlanId == plan_id
                          select new { maj_id = plan_maj.MajorId, maj_name = maj.Major }).ToList();

            var num_majors = 0;
            var num_minors = 0;

            foreach (var major in majors) {
                ++num_majors;
                var major_requirements = (from maj_req in _context.LjcMajorRequirements
                                          where maj_req.MajorId == major.maj_id
                                          select new { req_id = maj_req.CourseId, req_cat = maj_req.Category }).ToList();

                requirements.addCategory(major.maj_name + " Core");
                requirements.addCategory(major.maj_name + " Cognates");
                requirements.addCategory(major.maj_name + " Electives");

                foreach (var major_requirement in major_requirements) {
                    requirements.categories[major.maj_name + " " + major_requirement.req_cat].addCourse(major_requirement.req_id);
                }
            }

            var minors = (from plan_min in _context.LjcPlannedMinors
                          join min in _context.LjcMinors
                          on plan_min.MinorId equals min.MinorId
                          where plan_min.PlanId == plan_id
                          select new { min_id = plan_min.MinorId, min_name = min.Minor }).ToList();

            foreach (var minor in minors)
            {
                ++num_minors;
                var minor_requirements = (from min_req in _context.LjcMinorRequirements
                                          where min_req.MinorId == minor.min_id
                                          select new { req_id = min_req.CourseId }).ToList();

                var disp_min_name = minor.min_name + (minor.min_name == "Gen Eds" ? "" : " Minor");
                requirements.addCategory(disp_min_name);

                foreach (var minor_requirement in minor_requirements)
                {
                    requirements.categories[disp_min_name].addCourse(minor_requirement.req_id);
                }
            }

            //return Json(new {majors, minors} );
            return JsonConvert.SerializeObject(requirements);
            //return Json(requirements.categories);
        }
    }
}
