using Brutus.Data;
using Brutus.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class RodzicPanelController : Controller
    {
        private BrutusContext _context;
        public RodzicPanelController(BrutusContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Uczniowie(int id)
        {
           // Nie wiem co tu wpisać. LINQ
           return Index();
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