using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Ogloszenie ogloszenie) 
        {
            if (ModelState.IsValid)
            {
                _context.Ogloszenia.Add(ogloszenie);
                _context.SaveChanges();
                return RedirectToAction("Index", "Ogloszenie");
            }
            return View(ogloszenie);
        }
        public IActionResult Aktualnosci()
        {
            var ogloszenie = _context.Ogloszenia.ToList();
            return View(ogloszenie);
        }
    }
}
