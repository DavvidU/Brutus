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
            var ogloszenia = _context.Ogloszenia.ToList();
            return View(ogloszenia);
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
                _context.Ogloszenia.Add(ogloszenie);;
                _context.SaveChanges();
                return RedirectToAction("Aktualnosci");
            }
            return View(ogloszenie);
        }

        public IActionResult Aktualnosci()
        {
            var ogloszenia = _context.Ogloszenia.ToList();
            return View(ogloszenia);
        }

        public IActionResult AktualnosciEdit()
        {
            var ogloszenia = _context.Ogloszenia.ToList();
            return View(ogloszenia);
        }

        [HttpPost]
        public IActionResult AktualnosciEdit(int id)
        {
            var ogloszenieDoUsuniecia = _context.Ogloszenia.FirstOrDefault(o => o.ID_Ogloszenia == id);
            if (ogloszenieDoUsuniecia == null)
            {
                return NotFound();
            }

            _context.Ogloszenia.Remove(ogloszenieDoUsuniecia);
            _context.SaveChanges();
            return RedirectToAction("Aktualnosci");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var ogloszenie = _context.Ogloszenia.FirstOrDefault(o => o.ID_Ogloszenia == id);
            if (ogloszenie == null)
            {
                return NotFound();
            }
            return View(ogloszenie);
        }

        [HttpPost]
        public IActionResult Update(Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                var existingOgloszenie = _context.Ogloszenia.FirstOrDefault(o => o.ID_Ogloszenia == ogloszenie.ID_Ogloszenia);
                if (existingOgloszenie == null)
                {
                    return NotFound();
                }
                existingOgloszenie.Tresc = ogloszenie.Tresc;
                existingOgloszenie.Nauczyciel = ogloszenie.Nauczyciel;

                _context.SaveChanges();
                return RedirectToAction("Aktualnosci");
            }

            return View(ogloszenie);
        }
    }
}
