using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class RodzicPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Uczniowie()
        {
            return RedirectToAction("Index", "Uczniowie");
        }

        public IActionResult Wiadomosci()
        {
            return RedirectToAction("Index", "Wiadomosci");
        }

        public IActionResult Oceny()
        {
            return RedirectToAction("Index", "Oceny");
        }
    }
}