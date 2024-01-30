using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class RodziceController : Controller
    {
        private BrutusContext _context;
        public RodziceController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        /*public IActionResult Create(int idDodanegoKonta)
        {
            
             //     * Metoda tworzy rekord rodzica w Rodzice z referencją do Konta
             
            Rodzic rodzic = new Rodzic();
            rodzic.ID_Rodzica = idDodanegoKonta;
            rodzic.Konto = _context.Konta.FirstOrDefault(p => p.ID_Konta == idDodanegoKonta);

            _context.Rodzice.Add(rodzic);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }*/
    }
}
