using Microsoft.AspNetCore.Mvc;

namespace ChoicesExtrasManagement.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
