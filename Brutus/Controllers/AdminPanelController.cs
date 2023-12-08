using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Konta()
        {
            return RedirectToAction("Index", "Konta");
        }
        public IActionResult Predmioty()
        {
            return RedirectToAction("Index", "Przedmioty");
        }
    }
}
