using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Controllers
{
    public class UczniowieController : Controller
    {
        private BrutusContext _context;
        public UczniowieController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int idDodanegoKonta)
        {
            /*
             * Metoda tworzy rekord ucznia w Uczniowie z referencją do Konta
             */
            Uczen uczen = new Uczen();
            uczen.ID_Ucznia = idDodanegoKonta;
            uczen.Konto = _context.Konta.FirstOrDefault(p => p.ID_Konta == idDodanegoKonta);

            _context.Uczniowie.Add(uczen);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
        [HttpGet]
        public IActionResult ZarzadzanieUczniami()
        {
            // Uzyskaj liste kont, które należą do uczniow
            // i dolacz informacje o rodzicach i klasach poszczegolnych uczniow

            var listaUczniow = _context.Uczniowie
            .Select(u => new UczenWithKlasaWithRodzic
            {
                ID_Ucznia = u.ID_Ucznia,
                ImieUcznia = u.Konto.Imie,
                NazwiskoUcznia = u.Konto.Nazwisko,
                ID_Klasy = u.Klasa != null ? u.Klasa.ID_Klasy : (int?)null,
                NumerRoku = u.Klasa != null ? u.Klasa.NumerRoku : (int?)null,
                LiteraKlasy = u.Klasa != null ? u.Klasa.LiteraKlasy : null,
                ID_Rodzica = u.Rodzic != null ? u.Rodzic.ID_Rodzica : (int?)null,
                ImieRodzica = u.Rodzic != null ? u.Rodzic.Konto.Imie : null,
                NazwiskoRodzica = u.Rodzic != null ? u.Rodzic.Konto.Nazwisko : null
            }).ToList();

            return View(listaUczniow);
        }
        [HttpGet]
        public ActionResult UstawRodzica(int idUcznia)
        {
            var uczen = _context.Uczniowie.Include(u => u.Konto).FirstOrDefault(u => u.ID_Ucznia == idUcznia);
            if (uczen == null)
            {
                return NotFound();
            }

            var rodzice = _context.Rodzice.Include(r => r.Konto).ToList().Select(r => new
            {
                ID_Rodzica = r.ID_Rodzica,
                FullName = r.Konto.Imie + " " + r.Konto.Nazwisko
            });

            var rodziceSelectList = new SelectList(rodzice, "ID_Rodzica", "FullName");

            var viewModel = new SetRodzicViewModel
            {
                ID_Ucznia = idUcznia,
                ImieUcznia = uczen.Konto.Imie,
                NazwiskoUcznia = uczen.Konto.Nazwisko,
                Rodzice = rodziceSelectList
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult UstawRodzica(SetRodzicViewModel model)
        {
            // Znadz ucznia
            var uczen = _context.Uczniowie.FirstOrDefault(u => u.ID_Ucznia == model.ID_Ucznia);
            if (uczen != null)
            {
                //uczen.Rodzic.ID_Rodzica = model.WybranyRodzicID; // Przypisz ID wybranego rodzica do ucznia

                // Znajdz rodzica
                var rodzic = _context.Rodzice.Find(model.WybranyRodzicID);

                // Przypisz rodzica do ucznia
                uczen.Rodzic = rodzic;

                _context.SaveChanges(); // Zapisz zmiany w bazie danych

                return RedirectToAction("ZarzadzanieUczniami");
            }

            return NotFound();

            return View(model);
        }
        [HttpGet]
        public ActionResult PrzypiszDoKlasy(int idUcznia)
        {
            var uczen = _context.Uczniowie.Include(u => u.Konto).FirstOrDefault(u => u.ID_Ucznia == idUcznia);
            if (uczen == null)
            {
                return NotFound();
            }

            var klasy = _context.Klasy.ToList().Select(k => new
            {
                ID_Klasy = k.ID_Klasy,
                ClassInfo = k.NumerRoku.ToString() + " " + k.LiteraKlasy
            });

            var klasySelectList = new SelectList(klasy, "ID_Klasy", "ClassInfo");

            var viewModel = new SetKlasaViewModel
            {
                ID_Ucznia = idUcznia,
                ImieUcznia = uczen.Konto.Imie,
                NazwiskoUcznia = uczen.Konto.Nazwisko,
                Klasy = klasySelectList
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PrzypiszDoKlasy(SetKlasaViewModel model)
        {
            // Znadz ucznia
            var uczen = _context.Uczniowie.Find(model.ID_Ucznia);
            if (uczen != null)
            {
                //uczen.Klasa.ID_Klasy = model.WybranaKlasaID; // Przypisz ID wybranej klasy do ucznia

                // Znajdz klase
                var klasa = _context.Klasy.Find(model.WybranaKlasaID);

                // Przypisz klase do ucznia

                uczen.Klasa = klasa;

                _context.SaveChanges(); // Zapisz zmiany w bazie danych

                return RedirectToAction("ZarzadzanieUczniami");
            }

            return NotFound();

            return View(model);
        }
    }
}
