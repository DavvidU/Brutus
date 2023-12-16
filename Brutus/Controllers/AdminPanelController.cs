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
        public IActionResult Przedmioty()
        {
            return RedirectToAction("Index", "Przedmioty");
        }
    }
}
