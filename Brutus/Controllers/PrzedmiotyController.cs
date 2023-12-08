using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Brutus.DTOs;

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

            // Pobierz wszystkie rekordy z Przedmiot JOIN NauczycielPrzedmiot JOIN Nauczyciel po ID
            // Zastosowane Left Join (w LINQ == into)
            var przedmiotyZNauczycielami = (from przedmiot in _context.Przedmioty
                                            join nauczycielPrzedmiot in _context.NauczycielePrzedmioty
                                            on przedmiot.ID_Przedmiotu equals nauczycielPrzedmiot.Przedmiot.ID_Przedmiotu into przemiotLeftJoinPN
                                            from pLJPN in przemiotLeftJoinPN.DefaultIfEmpty()
                                            join nauczyciel in _context.Konta
                                            on (pLJPN == null ? (int?)null : pLJPN.Nauczyciel.ID_Nauczyciela) equals nauczyciel.ID_Konta into pLJPNLeftJoinNauczyciel
                                            from pLJPNLJN in pLJPNLeftJoinNauczyciel.DefaultIfEmpty()
                                            select new PrzedmiotWithNauczyciel
                                            {
                                                ID_Przedmiotu = przedmiot.ID_Przedmiotu,
                                                PrzedmiotNazwa = przedmiot.Nazwa,
                                                PrzedmiotOpis = przedmiot.Opis,
                                                ID_Nauczyciela = pLJPNLJN != null ? pLJPNLJN.ID_Konta : (int?)null,
                                                NauczycielImie = pLJPNLJN != null ? pLJPNLJN.Imie : null,
                                                NauczycielNazwisko = pLJPNLJN != null ? pLJPNLJN.Nazwisko : null
                                            }).ToList();
            return View(przedmiotyZNauczycielami);
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

    }
}