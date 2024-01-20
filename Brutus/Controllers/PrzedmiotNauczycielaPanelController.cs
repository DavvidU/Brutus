using Brutus.Models;
using Brutus.Data;
using Brutus.Services;
using Brutus.Services.SortowanieUczniow;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); }

            Przedmiot? przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) {  return NotFound(); }


            if (!CzyUdzielicDostep(idPrzedmiotu, idNauczyciela)) 
                return RedirectToAction("AccesDenied", "Home");

            return View(przedmiot);
        }
        public IActionResult WylistujUczniow(int idPrzedmiotu)
        {
            string userId = User.Identity.GetUserId();

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); }

            Przedmiot? przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            if (!CzyUdzielicDostep(idPrzedmiotu, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            /* Znajdz klase chodzaca na ten przedmiot */
            
            KlasaPrzedmiot powiazanieKlasaPrzedmiot = _context.KlasyPrzedmioty.Include(kp => kp.Klasa).
                First(kp => kp.Przedmiot != null && kp.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            Klasa klasa = powiazanieKlasaPrzedmiot.Klasa;

            // Znajdz uczniow przypisanych do tej klasy

            List<Uczen> uczniowie = _context.Uczniowie.Include(u => u.Konto).Where(u => u.Klasa != null &&
                                        u.Klasa.ID_Klasy == klasa.ID_Klasy).ToList();

            uczniowie = sortowanieUczniow.SortujUczniow(uczniowie, _context);

            // Znajdz konta tych uczniow

            List<Konto> kontaUczniow = new();

            foreach (Uczen uczen in uczniowie)
            {
                Konto kontoUcznia = _context.Konta.First(k => k.ID_Konta == uczen.ID_Ucznia);

                if(kontoUcznia != null)
                    kontaUczniow.Add(kontoUcznia);
            }

            ViewBag.IDPrzedmiotu = idPrzedmiotu;

            return View(kontaUczniow);
        }
        public IActionResult PrzegladajUczniaWPrzedmiocie()
        {
            string userId = User.Identity.GetUserId();

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); }

            return View();
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

            return RedirectToAction("WylistujUczniow", new { idPrzedmiotu = idPrzedmiotu });
        }
        
        private bool CzyUdzielicDostep(int idPrzedmiotu, int idNauczyciela)
        {
            NauczycielPrzedmiot powiazanieNauczycielPrzedmiot = _context.NauczycielePrzedmioty.
                Include(np => np.Nauczyciel).First(np => np.Przedmiot != null && np.Nauczyciel != null &&
                np.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            if (powiazanieNauczycielPrzedmiot == null ||
                powiazanieNauczycielPrzedmiot.Nauczyciel.ID_Nauczyciela != idNauczyciela)
                return false;
            else
                return true;
        }
    }
}
