using Microsoft.AspNetCore.Mvc;
using Brutus.Models;
using Brutus.Data;
using Brutus.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering; // do SelectList (wyswietlanie listy nauczycieli)

namespace Brutus.Controllers
{
    public class KlasyController : Controller
    {
        private BrutusContext _context;

        public KlasyController(BrutusContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //List<Klasa> klasy = _context.Klasy.ToList();

            // Pobierz wszystkie rekordy z tabeli Klasy JOIN Konta po ID (wychowawcy -> konta)
            // Zastosowane Left Join (w LINQ == into i '?')
            // into tymczasowa tabela, DefaultIfEmpty bo klasa moze nie miec wychowawcy
            var klasyZWychowawcami = (from klasa in _context.Klasy
                                      join konto in _context.Konta
                                      on klasa.Wychowawca.ID_Wychowawcy equals konto.ID_Konta into klasaLeftJoinKonto
                                      from kontoWychowawcy in klasaLeftJoinKonto.DefaultIfEmpty()
                                      select new KlasaWithWychowawca
                                      {
                                          ID_Klasy = klasa.ID_Klasy,
                                          NumerRoku = klasa.NumerRoku,
                                          LiteraKlasy = klasa.LiteraKlasy,
                                          ID_Wychowawcy = kontoWychowawcy.ID_Konta,
                                          WychowawcaImie = kontoWychowawcy.Imie,
                                          WychowawcaNazwisko = kontoWychowawcy.Nazwisko
                                      }).ToList();

            return View(klasyZWychowawcami);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Klasa klasa)
        {
            /*
             * Metoda dodaje klase do Klasy
             */
            if(ModelState.IsValid)
            {
                // Dodaj klase do Klasy
                _context.Klasy.Add(klasa);

                // Zapisz zmiany w bazie danych
                _context.SaveChanges();

                return RedirectToAction("Index", "Klasy");
            }

            return View(klasa);
        }

        [HttpGet]
        public IActionResult Update(int idKlasy)
        {
            var klasa = _context.Klasy.FirstOrDefault(p => p.ID_Klasy == idKlasy);
            if (klasa == null)
            {
                return NotFound();
            }

            return View(klasa);
        }
        [HttpPost]
        public IActionResult Update(Klasa klasa)
        {
            if (ModelState.IsValid)
            {
                var existingKlasa = _context.Klasy.FirstOrDefault(p => p.ID_Klasy == klasa.ID_Klasy);

                if (existingKlasa == null)
                {
                    return NotFound();
                }

                existingKlasa.NumerRoku = klasa.NumerRoku;
                existingKlasa.LiteraKlasy = klasa.LiteraKlasy;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(klasa);
        }

        [HttpPost]
        public IActionResult Delete(int idKlasy)
        {
            /*
             * Metoda usuwa klase z Klasy
             * , odpowednie rekordy z KlasyPrzedmioty
             * , wypisuje uczniow z usuwanej klasy
             * i usuwa odpowiedniego wychowawce w Wychowawcy (niewymagane w bazie, tylko logika nakazuje)
             * --- (w odwrotnej kolejnosci) ---
             */

            // Znajdź klase do usunięcia w Klasy
            var klasaDoUsuniecia = _context.Klasy.FirstOrDefault(k => k.ID_Klasy == idKlasy);
            if (klasaDoUsuniecia == null)
            {
                return NotFound();
            }

            /*
             * Usun wychowawce, ktory wychowywal usuwana klase
             */

            // Jesli klasa nie ma wychowawcy to nie ma kogo usuwac
            if (klasaDoUsuniecia.Wychowawca == null) { }
            else // jesli klasa ma wychowawce - usun go
            {
                // Znajdz wychowawce usuwanej klasy
                var wychowawcaDoUsuniecia = _context.Wychowawcy.FirstOrDefault(w => w.ID_Wychowawcy ==
                                                                        klasaDoUsuniecia.Wychowawca.ID_Wychowawcy);

                // Usun znalezionego wychowawce
                _context.Wychowawcy.Remove(wychowawcaDoUsuniecia);
            }

            /* 
             * Wypisywanie wszystkich uczniow z usuwanej klasy
             */

            // Pobierz liste uczniow przypisanych do usuwanej klasy
            var uczniowieDoWypisania = (from uczen in _context.Uczniowie
                                        where uczen.Klasa != null && uczen.Klasa.ID_Klasy == idKlasy
                                        select uczen).ToList();

            // Wypisz pozostalych uczniow z klasy
            foreach (Uczen uczen in uczniowieDoWypisania)
            {
                // Wypisz ucznia z usuwanej klasy
                uczen.Konto = null;
            }

            /*
             * Usuwanie odpowiednich rekordow z KlasyPrzedmioty
             */

            // Pobierz liste rekordow z KlasyPrzedmioty
            List<KlasaPrzedmiot> rekordyDoUsuniecia = (from rekord in _context.KlasyPrzedmioty
                                                       where rekord.Klasa != null && rekord.Klasa.ID_Klasy == idKlasy
                                                       select rekord).ToList();

            // Usun wszystkie rekordy z listy w KlasyPrzedmioty
            foreach (KlasaPrzedmiot rekord in rekordyDoUsuniecia)
            {
                _context.KlasyPrzedmioty.Remove(rekord);
            }

            /*
             * Usuwanie klasy z Klasy
             */

            // Usun klase z Klasy
            _context.Klasy.Remove(klasaDoUsuniecia);

            // Zapisz zmiany w bazie
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SetWychowawca(int idKlasy)
        {
            //Znajdz klase do przypisania w niej wychowawcy
            var klasa = _context.Klasy.FirstOrDefault(k => k.ID_Klasy == idKlasy);
            if (klasa == null) { return NotFound(); }

            //Uzyskaj liste kont, które należą do nauczycieli, TO DO: ktorzy nie sa wychowawcami
            List<Konto> nauczyciele = (from konto in _context.Konta
                                       join nauczyciel in _context.Nauczyciele
                                       on konto.ID_Konta equals nauczyciel.ID_Nauczyciela
                                       select konto).ToList();

            // Stworz VievModel prezentujacy klase i liste nauczycieli do wyboru
            var viewModel = new SetWychowawcaViewModel
            {
                Klasa = klasa,
                Nauczyciele = new SelectList(nauczyciele, "ID_Konta", "Nazwisko")
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SetWychowawca(SetWychowawcaViewModel model)
        {
            /*
             * Metoda aktualizuje wychowawce w klasie - Przy czym:
             * - Jeśli nowy wychowawca zastepuje starego, to starego nalezy usunac z Wychowawcy
             * - A nowego dodac.
             * - TO DO: !!! DODATKOWO: jesli nowy wychowawca jest 
             * 
             * Przeplyw czynnosci:
             * 1. Jesli zastepujemy starego wychowawce :
             *      a) Ustaw wychowawce w klasie na null
             *      b) usun go z Wychowawcy
             * 2. Dodaj nowego wychowawce (nauczyciela) do Wychowawcy
             * 3. Przypisz go jako nowego wychowawce klasy
             */

            // Znajdz odpowiednia klase

            var klasa = _context.Klasy.FirstOrDefault(k => k.ID_Klasy == model.Klasa.ID_Klasy);

            /*
             * 1. Usuwanie starego wychowawcy
             */

            // Jesli klasa ma dotychczasowego wychowawce
            if (klasa.Wychowawca != null)
            {
                // Zapisz ID starego wychowawcy w Wychowawcy
                int idStaregoWychowawcy = model.Klasa.Wychowawca.ID_Wychowawcy;

                // Ustaw wychowawce klasy na null
                klasa.Wychowawca = null;

                // Znajdz starego wychowawce w Wychowawcy
                var wychowawcaDoUsuniecia = _context.Wychowawcy.FirstOrDefault(w => w.ID_Wychowawcy == 
                                                                                    idStaregoWychowawcy);
                
                // Usun starego wychowawce z Wychowawcy
                _context.Wychowawcy.Remove(wychowawcaDoUsuniecia); // wywchowawca istnieje w Wychowawcy, jesli klasa go miala
            }

            /*
             * Dodawanie nowego wychowawcy do Wychowawcy
             */

            // Znajdź wybranego nauczyciela w Nauczyciele
            var wybranyNauczyciel = _context.Nauczyciele.FirstOrDefault(p =>
            p.ID_Nauczyciela == model.WybranyNauczycielID);

            // Dodaj go do wychowawcy
            Wychowawca wychowawca = new Wychowawca();
            wychowawca.ID_Wychowawcy = wybranyNauczyciel.ID_Nauczyciela;
            wychowawca.Nauczyciel = wybranyNauczyciel;
            
            _context.Wychowawcy.Add(wychowawca);

            var wychowawcaWBazie = _context.Wychowawcy.FirstOrDefault(w => w.ID_Wychowawcy == 
                                                                        wychowawca.ID_Wychowawcy);

            /*
             * Przypisywanie nowego wychowawcy do klasy
             */
            klasa.Wychowawca = wychowawca;
            //model.Klasa.Wychowawca.ID_Wychowawcy = wychowawca.ID_Wychowawcy;

            // Zapisz zmiant w bazie

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult PrzedmiotyKlasy(int idKlasy)
        {
            // Znajdz klase do wyswietlenia jej z przedmiotami
            var klasa = _context.Klasy.FirstOrDefault(k => k.ID_Klasy == idKlasy);
            if (klasa == null) { return NotFound(); }

            //Uzyskaj liste przedmiotow przypisanych do klasy
            var przedmiotyKlasy = (from klasyPrzedmioty in _context.KlasyPrzedmioty
                                   join przedmiot in _context.Przedmioty
                                   on klasyPrzedmioty.Przedmiot.ID_Przedmiotu equals przedmiot.ID_Przedmiotu
                                   where klasyPrzedmioty.Klasa.ID_Klasy == idKlasy
                                   select przedmiot).ToList();

            // Stworz VievModel do zaprezentowania
            var viewModel = new KlasaWithPrzedmioty
            {
                Klasa = klasa,
                Przedmioty = przedmiotyKlasy,
                WszystkiePrzedmioty = new SelectList(_context.Przedmioty.ToList(), "ID_Przedmiotu", "Nazwa")
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PrzypiszPrzedmiotDoKlasy(KlasaWithPrzedmioty model)
        {
            // Znajdz odpowiednia klase
            var klasa = _context.Klasy.FirstOrDefault(k => k.ID_Klasy == model.Klasa.ID_Klasy);
            if (klasa == null) { return NotFound(); }

            // Znajdz odpowiedni przedmiot
            var przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == model.WybranyPrzedmiotID);
            if (przedmiot == null) { return NotFound(); }

            // Sprawdz, czy przyporządkowanie już istnieje
            var istniejePrzyporzadkowanie = _context.KlasyPrzedmioty.Any(kp => kp.Klasa.ID_Klasy == model.Klasa.ID_Klasy && kp.Przedmiot.ID_Przedmiotu == model.WybranyPrzedmiotID);
            if (!istniejePrzyporzadkowanie)
            {
                // Dodanie nowego przyporządkowania
                // Dodaj rekord w KlasyPrzedmioty
                KlasaPrzedmiot kp = new KlasaPrzedmiot();
                kp.Przedmiot = przedmiot;
                kp.Klasa = klasa;
                _context.KlasyPrzedmioty.Add(kp);

                // Zapisz zmiany w bazie danych
                _context.SaveChanges();
            }

            
            return RedirectToAction("PrzedmiotyKlasy", new { idKlasy = model.Klasa.ID_Klasy });
        }
    }
}
