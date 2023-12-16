using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class OgloszeniaController : Controller
    {
        private BrutusContext _context;

        public OgloszeniaController(BrutusContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ogloszeniaList = _context.Ogloszenia.ToList();
            return View(ogloszeniaList);
        }

        public IActionResult Create(int idNauczyciela, string tresc)
        {
            Ogloszenie ogloszenie = new Ogloszenie();
            //ogloszenie.ID_Nauczyciela = idNauczyciela;
            ogloszenie.Tresc = tresc;
            
            _context.Ogloszenia.Add(ogloszenie);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
    }
}