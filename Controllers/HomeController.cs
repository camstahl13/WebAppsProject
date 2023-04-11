using Microsoft.AspNetCore.Mvc;

namespace ljcProject5.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("student"))
            {
                return Redirect("/Student/Ape?student=" + User.Identity.Name);
            }
            else if (User.IsInRole("faculty"))
            {
                return Redirect("/Faculty");
            }
            else if (User.IsInRole("admin"))
            {
                return Redirect("/Admin");
            }
            return Redirect("/Identity/Account/Login");
        }
    }
}
