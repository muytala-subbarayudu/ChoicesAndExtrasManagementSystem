using Microsoft.AspNetCore.Mvc;

namespace ChoicesExtrasManagement.Controllers
{
    public class CatalogueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyChoicesExtras()
        {
            return View();
        }

        public IActionResult MyChoicesExtrasSelected()
        {
            return View();
        }

    }
}
