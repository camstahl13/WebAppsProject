﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ljcProject5.Controllers
{
    public class FacultyController : Controller
    {
        [Authorize(Roles = "faculty,admin")]
        public IActionResult Index()
        {
            return View("~/Views/Faculty/Index.cshtml");
        }
    }
}