using Microsoft.AspNetCore.Mvc;
using SusanOsuolaleCV.Models;
using System.Collections.Generic;

namespace SusanOsuolaleCV.Controllers
{
    public class EducationController : Controller
    {
        public IActionResult Index()
        {
            var items = new List<EducationItem>
            {
                new EducationItem { Degree = "B.Eng. Electronic & Electrical Engineering", Institution = "Obafemi Awolowo University", Years = "2022", Details = "Control systems, instrumentation, electrical systems, electronics, communication systems." }
            };
            return View(items);
        }
    }
}
