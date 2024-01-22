using Brutus.Data;
using Brutus.Models;
using Brutus.Services.Command;
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

            return View(kontoPrzegladanegoUcznia);
        }
        public IActionResult GenerujZestawienie() 
        { 
            return View();
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
