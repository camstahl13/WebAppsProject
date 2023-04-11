using Microsoft.AspNetCore.Mvc;

namespace ljcProject5.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Faculty/Index.cshtml");
        }
    }
}