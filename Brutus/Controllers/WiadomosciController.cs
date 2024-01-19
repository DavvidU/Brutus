using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;


namespace Brutus.Controllers
{
    public class WiadomosciController : Controller
    {
        private BrutusContext _context;

        public WiadomosciController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Wiadomosc> listaWiadomosci = _context.Wiadomosci.ToList();
            return View(listaWiadomosci);
        }
        public IActionResult Wyslij()
        {

            return View();
        }
    }
}