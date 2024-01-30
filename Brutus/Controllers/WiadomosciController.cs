using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Brutus.Services.Command;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Controllers
{
    [Authorize]
    public class WiadomosciController : Controller
    {
        private BrutusContext _context;

        public WiadomosciController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idKonta = invoker.Invoke();

            if (idKonta == -1) { return NotFound(); }

            /* Pobieranie wiadomosci, ktorych odbiorca jest uzytkownik */

            /* TU BYCMOZE TRZEBA UZYC .Include().ThenInclude().ToList() // OgloszeniaController:52 */
            List<KontoWiadomosc> polaczeniaWiadomosciOdebraneWiadomosc = _context.KontaWiadomosci.
                                Include(w => w.Wiadomosc).Where(kw => kw.Odbiorca.ID_Konta == idKonta).ToList();

            List<Wiadomosc> wiadomosciOdebrane = new();

            foreach (var wiadomoscZOdbiarca in polaczeniaWiadomosciOdebraneWiadomosc)
            {
                wiadomosciOdebrane.Add(wiadomoscZOdbiarca.Wiadomosc);
            }

            /* Pobieranie mozliwych adresatow dla uzytkownika */

            List<Konto> mozliwiAdresaci = new();

            if (User.IsInRole("Nauczyciel"))
            {
                mozliwiAdresaci = (from konto in _context.Konta
                                   join rodzic in _context.Rodzice
                                   on konto.ID_Konta equals rodzic.ID_Rodzica
                                   select konto).ToList();
            }
            else if (User.IsInRole("Rodzic"))
            {
                mozliwiAdresaci = (from konto in _context.Konta
                                   join nauczyciel in _context.Nauczyciele
                                   on konto.ID_Konta equals nauczyciel.ID_Nauczyciela
                                   select konto).ToList();
            }

            var viewModel = new WiadomosciWithMozliwiAdresaci
            {
                Wiadomosci = wiadomosciOdebrane,
                MozliwiAdresaci = new SelectList(mozliwiAdresaci, "ID_Konta", "Nazwisko")
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Wyslij(WiadomosciWithMozliwiAdresaci model)
        {
            if (model.TrescWiadomosci == null || model.TrescWiadomosci.Length > 500)
                return RedirectToAction("Index");

            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idKonta = invoker.Invoke();

            if (idKonta == -1) { return NotFound(); }

            /* Dodanie wiadomosci do bazy */

            Konto kontoNadawcy = _context.Konta.FirstOrDefault(k => k.ID_Konta == idKonta);

            Wiadomosc wiadomosc = new Wiadomosc
            {
                Nadawca = kontoNadawcy,
                Data = DateTime.Now,
                Tresc = model.TrescWiadomosci
            };

            _context.Wiadomosci.Add(wiadomosc);
            _context.SaveChanges();

            /* Powiazanie adresata z wiadomoscia w bazie */

            Konto kontoOdbiorcy = _context.Konta.FirstOrDefault(k => k.ID_Konta == model.WybranyAdresatID);

            KontoWiadomosc polaczanieWiadomoscOdbiorca = new KontoWiadomosc
            {
                Odbiorca = kontoOdbiorcy,
                Wiadomosc = wiadomosc
            };

            _context.KontaWiadomosci.Add(polaczanieWiadomoscOdbiorca);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}