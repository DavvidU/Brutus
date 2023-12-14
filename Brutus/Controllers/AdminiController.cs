using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class AdminiController : Controller
    {
        private BrutusContext _context;
        public AdminiController(BrutusContext context)
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
             * Metoda tworzy rekord admina w Admini z referencją do Konta
             */
            Admin admin = new Admin();
            admin.ID_Admina = idDodanegoKonta;
            admin.Konto = _context.Konta.FirstOrDefault(p => p.ID_Konta == idDodanegoKonta);

            _context.Admini.Add(admin);
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Konta");
        }
    }
}
