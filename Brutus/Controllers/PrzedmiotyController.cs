using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Brutus.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering; // do SelectList (wyswietlanie listy nauczycieli)

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
            /*
             * Metoda dodaje przedmiot do Przedmioty
             * rekord w NauczycielePrzedmioty z nauczyiel == null
             */
            if (ModelState.IsValid)
            {
                // Dodaj przedmiot do Przedmioty
                _context.Przedmioty.Add(przedmiot);

                // Dodaj rekord w NauczycielePrzedmioty
                NauczycielPrzedmiot np = new NauczycielPrzedmiot();
                np.Przedmiot = przedmiot;
                np.Nauczyciel = null;
                _context.NauczycielePrzedmioty.Add(np);

                // Zapisz zmiany w bazie danych
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
            /*
             * Metoda usuwa przedmiot z Przedmioty
             * i odpowedni rekord z NauczycielePrzedmioty
             */

            // Znajdź przedmiot w Przedmioty do usunięcia
            var przedmiotDoUsuniecia = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == id);
            if (przedmiotDoUsuniecia == null)
            {
                return NotFound();
            }

            // Znajdź rekord w NauczycielePrzedmioty do usunięcia
            var rekordDoUsuniecia = _context.NauczycielePrzedmioty.FirstOrDefault(p =>
            p.Przedmiot.ID_Przedmiotu == przedmiotDoUsuniecia.ID_Przedmiotu);

            // Usuń rekord z NauczycielePrzedmioty
            _context.NauczycielePrzedmioty.Remove(rekordDoUsuniecia);

            // Usuń przedmiot z Przedmioty
            _context.Przedmioty.Remove(przedmiotDoUsuniecia);

            // Zapisz zmiany w bazie
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult SetNauczyciel(int idPrzedmiotu)
        {
            //Znajdz przedmiot do przypisania w nim nauczyciela
            var przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            //Uzyskaj liste kont, które należą do nauczycieli
            List<Konto> nauczyciele = (from konto in _context.Konta
                               join nauczyciel in _context.Nauczyciele
                               on konto.ID_Konta equals nauczyciel.ID_Nauczyciela
                               select konto).ToList();

            
            var viewModel = new SetNauczycielViewModel
            {
                Przedmiot = przedmiot,
                Nauczyciele = new SelectList(nauczyciele, "ID_Konta", "Nazwisko")
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SetNauczyciel(SetNauczycielViewModel model)
        {
            /*
             * Metoda aktualizuje rekord w NauczycielePrzedmioty określający kto prowadzi dany przedmiot
             */

            // Znajdź odpowiedni rekord w NauczycielePrzedmioty
            var rekordDoModyfikacji = _context.NauczycielePrzedmioty.FirstOrDefault(p => 
            p.Przedmiot.ID_Przedmiotu == model.Przedmiot.ID_Przedmiotu);

            //if (rekordDoModyfikacji == null) { return NotFound(); }

            // Znajdź wybranego nauczyciela w Nauczyciele
            var wybranyNauczyciel = _context.Nauczyciele.FirstOrDefault(p =>
            p.ID_Nauczyciela == model.WybranyNauczycielID);

            //if (wybranyNauczyciel == null) { return NotFound(); }

            // Przypisz nauczyciela do rekordu w NauczycielePrzedmioty
            rekordDoModyfikacji.Nauczyciel = wybranyNauczyciel;

            // Zapisz zmiant w bazie

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}