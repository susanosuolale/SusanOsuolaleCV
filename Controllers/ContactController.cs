using Microsoft.AspNetCore.Mvc;
using SusanOsuolaleCV.Models;

namespace SusanOsuolaleCV.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

 
    }
}
