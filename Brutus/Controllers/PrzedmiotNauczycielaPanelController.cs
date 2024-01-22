using Brutus.Models;
using Brutus.Data;
using Brutus.Services.SortowanieUczniow;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Brutus.Services.Command;

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


            if (!CzyUdzielicDostep(idPrzedmiotu, idNauczyciela)) 
                return RedirectToAction("AccesDenied", "Home");

            return View(przedmiot);
        }
        public IActionResult WylistujUczniow(int idPrzedmiotu)
        {
            /* TO SIE ZMIENI JAK ADAM UZYJE COMMAND */

            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();

            if (idNauczyciela == -1) { return NotFound(); }

            Przedmiot? przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            if (!CzyUdzielicDostep(idPrzedmiotu, idNauczyciela))
                return RedirectToAction("AccesDenied", "Home");

            /* Znajdz klase chodzaca na ten przedmiot */
            
            KlasaPrzedmiot powiazanieKlasaPrzedmiot = _context.KlasyPrzedmioty.Include(kp => kp.Klasa).
                FirstOrDefault(kp => kp.Przedmiot != null && kp.Przedmiot.ID_Przedmiotu == idPrzedmiotu);

            Klasa klasa = powiazanieKlasaPrzedmiot.Klasa;

            // Znajdz uczniow przypisanych do tej klasy

            List<Uczen> uczniowie = _context.Uczniowie.Include(u => u.Konto).Where(u => u.Klasa != null &&
                                        u.Klasa.ID_Klasy == klasa.ID_Klasy).ToList();

            uczniowie = sortowanieUczniow.SortujUczniow(uczniowie, _context, idPrzedmiotu);

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

            if (!CzyUdzielicDostep(idPrzedmiotu, idNauczyciela))
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
        
        private bool CzyUdzielicDostep(int idPrzedmiotu, int idNauczyciela)
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
