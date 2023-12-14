using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class UczniowieController : Controller
    {
        private BrutusContext _context;
        public UczniowieController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int idDodanegoKonta)
        {
            /*
             * Metoda tworzy rekord ucznia w Uczniowie z referencją do Konta
             */
            Uczen uczen = new Uczen();
            uczen.ID_Ucznia = idDodanegoKonta;
            uczen.Konto = _context.Konta.FirstOrDefault(p => p.ID_Konta == idDodanegoKonta);

            _context.Uczniowie.Add(uczen);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
    }
}
