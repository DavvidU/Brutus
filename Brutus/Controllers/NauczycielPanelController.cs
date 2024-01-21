using Brutus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Brutus.Controllers
{
    [Authorize(Roles = "Nauczyciel")]
    public class NauczycielPanelController : Controller
    {
        private BrutusContext _context;

        public NauczycielPanelController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult PokazPrzedmioty()
        {
            return RedirectToAction("ReadForNauczyciel", "Przedmioty");
        }
        public IActionResult Ogloszenia()
        {
            return RedirectToAction("Index", "Ogloszenia");
        }
        public IActionResult Wiadomosc()
        {
            return RedirectToAction("Index", "Wiadomosci");
        }
        public IActionResult Test()
        {
            return RedirectToAction("Index","Testy");
        }
    }
}
