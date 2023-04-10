using ljcProject5.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace ljcProject5.Controllers
{
    public class RedirectController : Controller
    {
        public async Task<IActionResult> Enter()
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
            return Unauthorized();
        }
    }
}
