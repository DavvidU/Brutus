using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class UczenPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Oceny()
        {
            return RedirectToAction("Index", "Oceny");
        }

        public IActionResult Testy()
        {
            return RedirectToAction("Index", "Testy");
        }

        public IActionResult Wiadomosci()
        {
            return RedirectToAction("Index", "Wiadomosci");
        }
    }
}