using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Brutus.Controllers
{
    public class PrzedmiotyController : Controller
    {
        private BrutusContext _context;
        public PrzedmiotyController(BrutusContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Read();
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Przedmiot przedmiot)
        {
            if (ModelState.IsValid)
            {
                _context.Przedmioty.Add(przedmiot);
                _context.SaveChanges();
                return RedirectToAction("Index", "Przedmioty");
            }
            return View(przedmiot);
        }
        public IActionResult Read()
        {
            var przedmioty = _context.Przedmioty.ToList();
            return View(przedmioty);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == id);
            if (przedmiot == null)
            {
                return NotFound();
            }

            return View("Update", przedmiot);
        }
        [HttpPost]
        public IActionResult Update(Przedmiot przedmiot)
        {
            if (ModelState.IsValid)
            {
                var existingPrzedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == przedmiot.ID_Przedmiotu);

                if (existingPrzedmiot == null)
                {
                    return NotFound();
                }

                existingPrzedmiot.Nazwa = przedmiot.Nazwa;
                existingPrzedmiot.Opis = przedmiot.Opis;

                _context.SaveChanges();

                return RedirectToAction("Index"); 
            }

            return View(przedmiot);
        }
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var przedmiotDoUsuniecia = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == id);
            if (przedmiotDoUsuniecia == null)
            {
                return NotFound();
            }

            _context.Przedmioty.Remove(przedmiotDoUsuniecia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult ReadForNauczyciel()
        {
            var przedmioty = _context.Przedmioty.ToList();
            return View(przedmioty);
        }
    }
}