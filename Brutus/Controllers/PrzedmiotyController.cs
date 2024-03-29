﻿using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Brutus.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering; // do SelectList (wyswietlanie listy nauczycieli)
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Brutus.Services.Command;

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
            // Zastosowane Left Join (w LINQ == into i '?')
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
             * ----- TRZEBA JESZCZE -----
             * - usunac odpowiednie rekordy z KlasyPrzedmioty
             * - usunac odpowiednie rekordy z PrzedmiotyZalaczniki
             * - wynullowac ref. do przemiotu w ocenach z Oceny, ktore sa wystawione z tego przedmiotu
             */
            
            // Znajdź przedmiot w Przedmioty do usunięcia
            var przedmiotDoUsuniecia = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == id);
            if (przedmiotDoUsuniecia == null)
            {
                return NotFound();
            }

            /*
             * Usuwanie rekordu z NauczycielePrezdmioty
             */

            // Znajdź rekord w NauczycielePrzedmioty do usunięcia
            var rekordDoUsuniecia = _context.NauczycielePrzedmioty.FirstOrDefault(p =>
            p.Przedmiot.ID_Przedmiotu == przedmiotDoUsuniecia.ID_Przedmiotu);

            // Usuń rekord z NauczycielePrzedmioty !!!!!!! TO TEZ BUGUJE RACZEJ PRZY >1 NAUCZYCIELU
            _context.NauczycielePrzedmioty.Remove(rekordDoUsuniecia);

            /*
             * Usuwanie rekordow z KlasyPrzedmioty
             */



            /*
             * Usuwanie rekordow z PrzedmiotyZalaczniki
             */



            /*
             * Wynullowanie refernencji na przedmiot w odpowiednich ocenach w Oceny
             */



            /*
             * Usuwanie przedmiotu
             */

            // Usuń przedmiot z Przedmioty
            _context.Przedmioty.Remove(przedmiotDoUsuniecia);

            // Zapisz zmiany w bazie
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult SetNauczyciel(int idPrzedmiotu)
        {
            // Znajdz przedmiot do przypisania w nim nauczyciela
            var przedmiot = _context.Przedmioty.FirstOrDefault(p => p.ID_Przedmiotu == idPrzedmiotu);
            if (przedmiot == null) { return NotFound(); }

            // Uzyskaj liste kont, które należą do nauczycieli
            List<Konto> nauczyciele = (from konto in _context.Konta
                               join nauczyciel in _context.Nauczyciele
                               on konto.ID_Konta equals nauczyciel.ID_Nauczyciela
                               select konto).ToList();

            // Stworz VievModel prezentujacy przedmiot i liste nauczycieli do wyboru
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
            // Domyslnie kazdy przedmiot ma rekord w NauczycielePrzedmioty (TO CHYBA ZLE!!!!
            // Patrz Klasy/Delete, Przedmioty/Delete. Mozliwe BUGI dla wiecje niz jednego nauczyciela w Przedmiocie)

            /*
             * Metoda aktualizuje rekord w NauczycielePrzedmioty określający kto prowadzi dany przedmiot
             */

            // Znajdź odpowiedni rekord w NauczycielePrzedmioty
            var rekordDoModyfikacji = _context.NauczycielePrzedmioty.FirstOrDefault(p => 
            p.Przedmiot.ID_Przedmiotu == model.Przedmiot.ID_Przedmiotu);

            if (rekordDoModyfikacji == null) { return NotFound(); }

            // Znajdź wybranego nauczyciela w Nauczyciele
            var wybranyNauczyciel = _context.Nauczyciele.FirstOrDefault(p =>
            p.ID_Nauczyciela == model.WybranyNauczycielID);

            if (wybranyNauczyciel == null) { return NotFound(); }

            // Przypisz nauczyciela do rekordu w NauczycielePrzedmioty
            rekordDoModyfikacji.Nauczyciel = wybranyNauczyciel;

            // Zapisz zmiant w bazie

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Nauczyciel")]
        public IActionResult ReadForNauczyciel() // Przedmioty/ReadForNauczyciel?idNauczyciela=28
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();

            if (idNauczyciela == -1) { return NotFound(); }

            //if (!AccesVerification.Verify(idNauczyciela, userId, _context))
                //return RedirectToAction("AccesDenied", "Home");

            // Znajdz konto nauczyciela do wyswietlenia go z jego przedmiotami
            Konto kontoNauczyciela = _context.Konta.FirstOrDefault(k => k.ID_Konta == idNauczyciela);
            if (kontoNauczyciela == null) { return NotFound(); }

            // Znajdz przedmioty nalezace do tego nauczyciela
            List<Przedmiot> przedmiotyNauczyciela = (from nauczycielePrzedmioty in _context.NauczycielePrzedmioty
                                                     join przedmiot in _context.Przedmioty
                                                     on nauczycielePrzedmioty.Przedmiot.ID_Przedmiotu equals przedmiot.ID_Przedmiotu
                                                     where nauczycielePrzedmioty.Nauczyciel.ID_Nauczyciela == idNauczyciela
                                                     select przedmiot).ToList();

            // Stworz VievModel do zaprezentowania
            var viewModel = new NauczycielWithPrzedmioty
            {
                KontoNauczyciela = kontoNauczyciela,
                PrzedmiotyNauczyciela = przedmiotyNauczyciela
            };
            
            return View(viewModel);
        }
    }
}