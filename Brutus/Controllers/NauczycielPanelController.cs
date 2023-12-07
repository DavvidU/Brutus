using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class NauczycielPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Ogloszenia()
        {
            return RedirectToAction("Index", "Ogloszenia");
        }

        public IActionResult Testy()
        {
            return RedirectToAction("Index", "Testy");
        }

        public IActionResult Oceny()
        {
            return RedirectToAction("Index", "Oceny");
        }
    }
}