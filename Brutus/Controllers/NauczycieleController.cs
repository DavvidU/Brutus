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
            Nauczyciel nauczyciel = new Nauczyciel();
            nauczyciel.ID_Nauczyciela = idDodanegoKonta;

            _context.Nauczyciele.Add(nauczyciel);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
   
    }
}
