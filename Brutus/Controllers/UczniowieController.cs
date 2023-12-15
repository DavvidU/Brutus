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
            Uczen uczen = new Uczen();
            uczen.ID_Ucznia = idDodanegoKonta;

            _context.Uczniowie.Add(uczen);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
    }
}
