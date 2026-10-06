using Microsoft.AspNetCore.Mvc;
using SusanOsuolaleCV.Models;

namespace SusanOsuolaleCV.Controllers
{
    public class TaxController : Controller
    {
        public IActionResult Index()
        {
            var model = new TaxCalculationViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Calculate(TaxCalculationViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            model.Calculate();
            return View("Index", model);
        }
    }
}
