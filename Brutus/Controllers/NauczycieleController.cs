using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class NauczycieleController : Controller
    {
        private BrutusContext _context;
        public NauczycieleController(BrutusContext context)
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
             * Metoda tworzy rekord nauczyciela w Nauczyiele z referencją do Konta
             */
            Nauczyciel nauczyciel = new Nauczyciel();
            nauczyciel.ID_Nauczyciela = idDodanegoKonta;
            nauczyciel.Konto = _context.Konta.FirstOrDefault(p => p.ID_Konta ==  idDodanegoKonta);

            _context.Nauczyciele.Add(nauczyciel);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
   
    }
}
