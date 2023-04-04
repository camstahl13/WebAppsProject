using Microsoft.AspNetCore.Mvc;

namespace ljcProject5.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
