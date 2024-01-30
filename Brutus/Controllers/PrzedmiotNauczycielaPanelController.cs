using Brutus.Models;
using Brutus.Data;
using Brutus.Services.SortowanieUczniow;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Brutus.Services.Command;
using Brutus.Services.Zestawienie;

namespace Brutus.Controllers
{
    [Authorize(Roles = "Nauczyciel")]
    public class PrzedmiotNauczycielaPanelController : Controller
    {
        ISortowanieUczniow sortowanieUczniow = new SortowanieAlfabetyczne();

        private BrutusContext _context;
        public PrzedmiotNauczycielaPanelController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index(int idPrzedmiotu)
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();
            if (idNauczyciela == -1) { return NotFound(); }

            Przedmiot? przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) {  return NotFound(); }


            if (!CzyUdzielicDostepDoPrzedmiotu(idPrzedmiotu, idNauczyciela)) 
                return RedirectToAction("AccesDenied", "Home");

            return View(przedmiot);
        }
        public IActionResult GenerujZestawienieZPrzedmiotu(int idPrzedmiotu, bool czyZawieraKomentarze,
            bool czyZawieraWagi, bool czyPorownanieNaTleKlasy, DateTime dataPoczatkowa, DateTime dataKoncowa)
        {
            string userId = User.Identity.GetUserId(); // Pobierz torzsamosc nauczyciela generujacego zestawienie

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); } // Pobierz biznesowe ID nauczyciela

            if (!CzyUdzielicDostepDoPrzedmiotu(idPrzedmiotu, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            /* Znajdz klase zapisana na rzadany przedmiot */

            KlasaPrzedmiot powiazanieRzadanyPrzedmiotKlasa = _context.KlasyPrzedmioty.
                Include(kp => kp.Klasa).FirstOrDefault(kp => kp.Przedmiot != null && kp.Klasa != null &&
                kp.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            if (powiazanieRzadanyPrzedmiotKlasa == null)
                return NotFound();

            int idKlasy = powiazanieRzadanyPrzedmiotKlasa.Klasa.ID_Klasy;

            /* Znajdz wszystkich uczniow z tej klasy */

            List<Uczen> uczniowie = _context.Uczniowie.Include(u => u.Konto).Where(u => u.Klasa != null &&
                                        u.Klasa.ID_Klasy == idKlasy).ToList();

            List<Zestawienie> zestawienieZPrzedmiotu = new();

            /* Przygotowanie zestawienia dla kazdego z uczniow */

            Przedmiot przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            Konto kontoUcznia;
            List<Ocena> ocenyUcznia;
            Zestawienie zestawienieUcznia;

            foreach (Uczen uczen in uczniowie)
            {
                kontoUcznia = _context.Konta.FirstOrDefault(k => k.ID_Konta == uczen.ID_Ucznia);
                if (kontoUcznia == null) { return NotFound(); }

                ocenyUcznia = _context.Oceny.Where(o => o.Uczen != null &&
                                                            o.Uczen.ID_Ucznia == uczen.ID_Ucznia &&
                                                            o.Data.Date >= dataPoczatkowa.Date &&
                                                            o.Data.Date <= dataKoncowa.Date).ToList();

                zestawienieUcznia = new ZestawienieUcznia(uczen.ID_Ucznia, kontoUcznia.Imie, kontoUcznia.Nazwisko,
                                                            idPrzedmiotu, przedmiot.Nazwa, ocenyUcznia);
                if (czyZawieraWagi)
                    zestawienieUcznia = new WagiOcen(zestawienieUcznia);
                if (czyZawieraKomentarze)
                    zestawienieUcznia = new KomentarzeOcen(zestawienieUcznia);
                if (czyPorownanieNaTleKlasy)
                    zestawienieUcznia = new TrendRozwojuNaTleKlasy(zestawienieUcznia, _context);

                zestawienieZPrzedmiotu.Add(zestawienieUcznia);
            }

            return View(zestawienieZPrzedmiotu);
        }
        public IActionResult WylistujUczniow(int idPrzedmiotu)
        {
            /* Znajdz uczniow chodzacych na przedmiot i wylistuj ich za pomoca strategii sortowania */

            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();

            if (idNauczyciela == -1) { return NotFound(); }

            Przedmiot? przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            if (!CzyUdzielicDostepDoPrzedmiotu(idPrzedmiotu, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            /* Znajdz klase chodzaca na ten przedmiot */
            
            KlasaPrzedmiot powiazanieKlasaPrzedmiot = _context.KlasyPrzedmioty.Include(kp => kp.Klasa).
                FirstOrDefault(kp => kp.Przedmiot != null && kp.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            Klasa klasa = powiazanieKlasaPrzedmiot.Klasa;

            // Znajdz uczniow przypisanych do tej klasy

            List<Uczen> uczniowie = _context.Uczniowie.Include(u => u.Konto).Where(u => u.Klasa != null &&
                                        u.Klasa.ID_Klasy == klasa.ID_Klasy).ToList();

            uczniowie = sortowanieUczniow.SortujUczniow(uczniowie, _context, idPrzedmiotu); // Uzycie strategii

            // Znajdz konta tych uczniow

            List<Konto> kontaUczniow = new();

            foreach (Uczen uczen in uczniowie)
            {
                Konto kontoUcznia = _context.Konta.FirstOrDefault(k => k.ID_Konta == uczen.ID_Ucznia);

                if(kontoUcznia != null)
                    kontaUczniow.Add(kontoUcznia);
            }

            ViewBag.IdPrzedmiotu = idPrzedmiotu;

            return View("WylistujUczniow" , kontaUczniow);
        }
        public IActionResult PrzegladajUczniaWPrzedmiocie(int idPrzedmiotu, int idUcznia)
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();

            if (idNauczyciela == -1) { return NotFound(); }

            if (!CzyUdzielicDostepDoPrzedmiotu(idPrzedmiotu, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            return RedirectToAction("Index", "PodgladUczniaWPrzedmiocie", new { idPrzedmiotu = idPrzedmiotu, idUcznia = idUcznia }) ;
        }
        public IActionResult ZmienSposobSortowaniaUczniow(int idPrzedmiotu, string sposobSortowania)
        {
            switch(sposobSortowania)
            {
                case "Alfabetycznie":
                    sortowanieUczniow = new SortowanieAlfabetyczne();
                    break;
                case "Oceny":
                    sortowanieUczniow = new SortowanieWzgledemOcen();
                    break;
                default:
                    sortowanieUczniow = new SortowanieAlfabetyczne();
                    break;
            }

            return WylistujUczniow(idPrzedmiotu);
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
    }
}
