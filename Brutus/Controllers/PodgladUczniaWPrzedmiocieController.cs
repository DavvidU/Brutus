using Brutus.DTOs;
using Brutus.Data;
using Brutus.Models;
using Brutus.Services.Command;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Brutus.Services.Zestawienie;

namespace Brutus.Controllers
{
    [Authorize(Roles = "Nauczyciel")]
    public class PodgladUczniaWPrzedmiocieController : Controller
    {
        private BrutusContext _context;
        public PodgladUczniaWPrzedmiocieController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index(int idPrzedmiotu, int idUcznia)
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();
            if (idNauczyciela == -1) { return NotFound(); }

            Uczen przegladanyUczen = _context.Uczniowie.Include(u => u.Klasa).FirstOrDefault(u => u.ID_Ucznia == idUcznia);
            if (przegladanyUczen == null)
                return NotFound();

            if (!CzyUdzielicDostep(idPrzedmiotu, przegladanyUczen, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            Konto kontoPrzegladanegoUcznia = _context.Konta.FirstOrDefault(k => k.ID_Konta == przegladanyUczen.ID_Ucznia);

            ViewBag.IDPrzedmiotu = idPrzedmiotu;

            return View(kontoPrzegladanegoUcznia);
        }
        public IActionResult GenerujZestawienie(int idPrzedmiotu, int idUcznia, bool czyZawieraKomentarze, 
            bool czyZawieraWagi, bool czyPorownanieNaTleKlasy, DateTime dataPoczatkowa, DateTime dataKoncowa) 
        {
            string userId = User.Identity.GetUserId(); // Pobierz torzsamosc nauczyciela generujacego zestawienie

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); } // Pobierz biznesowe ID nauczyciela

            Uczen uczen = _context.Uczniowie.Include(u => u.Klasa).FirstOrDefault(u => u.ID_Ucznia == idUcznia);
            if (uczen == null) // Znajdz ucznia dla ktorego ma byc wygenerowane zestawienie
                return NotFound();

            if (!CzyUdzielicDostep(idPrzedmiotu, uczen, idNauczyciela)) // Czy nauczyciel jest uprawniony do akcji
                return RedirectToAction("AccesDenied", "Home");

            /* Obiekty ktorych dotyczy zestawienie */

            Konto kontoUcznia = _context.Konta.FirstOrDefault(k => k.ID_Konta == idUcznia);
            if (kontoUcznia == null) { return NotFound(); }

            Przedmiot przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            List<Ocena> ocenyUcznia = _context.Oceny.Where(o => o.Uczen != null && 
                                                            o.Uczen.ID_Ucznia == idUcznia &&
                                                            o.Data.Date >= dataPoczatkowa.Date &&
                                                            o.Data.Date <= dataKoncowa.Date).ToList();

            Zestawienie zestawienie = new ZestawienieUcznia(idUcznia, kontoUcznia.Imie, kontoUcznia.Nazwisko, 
                                                            idPrzedmiotu, przedmiot.Nazwa, ocenyUcznia);

            /* Dekorowanie zestawienia w rzadane przez nauczyiela informacje */

            if (czyZawieraWagi)
                zestawienie = new WagiOcen(zestawienie);
            if (czyZawieraKomentarze)
                zestawienie = new KomentarzeOcen(zestawienie);
            if (czyPorownanieNaTleKlasy)
                zestawienie = new TrendRozwojuNaTleKlasy(zestawienie, _context);

            return View(zestawienie);
        }
        [HttpGet]
        public IActionResult DodajOcene(int idPrzedmiotu, int idUcznia)
        {
            string userId = User.Identity.GetUserId();

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); }

            Uczen uczen = _context.Uczniowie.Include(u => u.Klasa).FirstOrDefault(u => u.ID_Ucznia == idUcznia);
            if (uczen == null)
                return NotFound();

            if (!CzyUdzielicDostep(idPrzedmiotu, uczen, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            Nauczyciel nauczyciel = _context.Nauczyciele.FirstOrDefault(n => n.ID_Nauczyciela == idNauczyciela);
            if (nauczyciel == null)
                return NotFound();

            Przedmiot przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null)
                return NotFound();

            var viewModel = new DodawanieOcenyViewModel(uczen, nauczyciel, przedmiot);

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult DodajOcene(DodawanieOcenyViewModel model)
        {
            if (model.Uczen != null && model.Nauczyciel != null && model.Przedmiot != null)
            {
                Ocena nowaOcena = new Ocena
                {
                    Wartosc = model.Wartosc,
                    Waga = model.Waga,
                    Komentarz = model.Komentarz,
                    Uczen = _context.Uczniowie.Find(model.Uczen.ID_Ucznia),
                    Nauczyciel = _context.Nauczyciele.Find(model.Nauczyciel.ID_Nauczyciela),
                    Przedmiot = _context.Przedmioty.Find(model.Przedmiot.ID_Przedmiotu),
                    Data = DateTime.Now
                };

                _context.Oceny.Add(nowaOcena);
                _context.SaveChanges();

                return RedirectToAction("Index", new
                {
                    idPrzedmiotu = model.Przedmiot.ID_Przedmiotu,
                    idUcznia = model.Uczen.ID_Ucznia
                });
            }
            else
                return NotFound();
            
        }

        private bool CzyUdzielicDostep(int idPrzedmiotu, Uczen rzadanyUczen, int idNauczyciela)
        {
            if (!CzyUdzielicDostepDoPrzedmiotu(idPrzedmiotu, idNauczyciela) ||
                !CzyUdzielicDostepDoUcznia(rzadanyUczen, idPrzedmiotu))
                return false;
            else
                return true;
        }
        private bool CzyUdzielicDostepDoPrzedmiotu(int idPrzedmiotu, int idNauczyciela)
        {
            // Czy nauczyciel prowadzi rzadany przedmiot

            NauczycielPrzedmiot powiazanieNauczycielPrzedmiot = _context.NauczycielePrzedmioty.
                Include(np => np.Nauczyciel).FirstOrDefault(np => np.Przedmiot != null && np.Nauczyciel != null &&
                np.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            if (powiazanieNauczycielPrzedmiot == null ||
                powiazanieNauczycielPrzedmiot.Nauczyciel.ID_Nauczyciela != idNauczyciela)
                return false;
            else
                return true;
        }
        private bool CzyUdzielicDostepDoUcznia(Uczen rzadanyUczen, int idPrzedmiotu)
        {
            /* 
             * Czy rzadany uczen nalezy do klasy zapisanej na rzadany przedmiot
            *  (Wywolywane po weryfikacji dostepu do rzadanego przedmiotu)
            */

            if (rzadanyUczen.Klasa == null)
                return false;

            // Znajdz klase zapisana na rzadany przedmiot

            KlasaPrzedmiot powiazanieRzadanyPrzedmiotKlasa = _context.KlasyPrzedmioty.
                Include(kp => kp.Klasa).FirstOrDefault(kp => kp.Przedmiot != null && kp.Klasa != null &&
                kp.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            if (powiazanieRzadanyPrzedmiotKlasa == null)
                return false;

            // Sprawdz czy rzadany uczen nalezy do tel klasy

            if (rzadanyUczen.Klasa.ID_Klasy == powiazanieRzadanyPrzedmiotKlasa.Klasa.ID_Klasy)
                return true;
            else
                return false;
        }
    }
}
