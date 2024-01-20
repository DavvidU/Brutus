using Brutus.Data;
using Brutus.Models;
using Brutus.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            int idNauczyciela = IdTranslator.TranslateToBusinessId(userId, _context);
            if (idNauczyciela == -1) { return NotFound(); }

            if(!CzyUdzielicDostep(idPrzedmiotu, idUcznia, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            return View();
        }

        private bool CzyUdzielicDostep(int idPrzedmiotu, int idUcznia, int idNauczyciela)
        {
            if (!CzyUdzielicDostepDoPrzedmiotu(idPrzedmiotu, idNauczyciela) ||
                !CzyUdzielicDostepDoUcznia(idUcznia, idPrzedmiotu))
                return false;
            else
                return true;
        }
        private bool CzyUdzielicDostepDoPrzedmiotu(int idPrzedmiotu, int idNauczyciela)
        {
            // Czy nauczyciel prowadzi rzadany przedmiot

            NauczycielPrzedmiot powiazanieNauczycielPrzedmiot = _context.NauczycielePrzedmioty.
                Include(np => np.Nauczyciel).First(np => np.Przedmiot != null && np.Nauczyciel != null &&
                np.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            if (powiazanieNauczycielPrzedmiot == null ||
                powiazanieNauczycielPrzedmiot.Nauczyciel.ID_Nauczyciela != idNauczyciela)
                return false;
            else
                return true;
        }
        private bool CzyUdzielicDostepDoUcznia(int idUcznia, int idPrzedmiotu)
        {
            /* 
             * Czy rzadany uczen nalezy do klasy zapisanej na rzadany przedmiot
            *  (Wywolywane po weryfikacji dostepu do rzadanego przedmiotu)
            */

            Uczen rzadanyUczen = _context.Uczniowie.Include(u => u.Klasa).FirstOrDefault(u => u.ID_Ucznia == idUcznia);

            if (rzadanyUczen == null || rzadanyUczen.Klasa == null)
                return false;

            // Znajdz klase zapisana na rzadany przedmiot

            KlasaPrzedmiot powiazanieRzadanyPrzedmiotKlasa = _context.KlasyPrzedmioty.
                Include(kp => kp.Klasa).First(kp => kp.Przedmiot != null && kp.Klasa != null &&
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
