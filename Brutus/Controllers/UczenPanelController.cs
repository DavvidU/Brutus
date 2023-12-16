using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class UczenPanelController : Controller
    {
        private BrutusContext _context;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Oceny(int idUcznia)
        {
            /*
             NullReferenceException: Object reference not set to an instance of an object.
               Brutus.Controllers.UczenPanelController.Oceny(int idUcznia) in UczenPanelController.cs
                           Konto kontoUcznia = _context.Konta.FirstOrDefault(k => k.ID_Konta == idUcznia);
            */
            Konto kontoUcznia = _context.Konta.FirstOrDefault(k => k.ID_Konta == idUcznia);
            if (kontoUcznia == null)
            {
                return NotFound();
            }
            List<Ocena> ocenyUcznia = (from uczniowieOceny in _context.UczniowieOceny
                    join ocena in _context.Oceny
                        on uczniowieOceny.Ocena.ID_Oceny equals ocena.ID_Oceny
                        where uczniowieOceny.Uczen.ID_Ucznia == idUcznia
                            select ocena).ToList();
            
            var ViewModel = new UczenWithOceny
            {
                KontoUcznia = kontoUcznia,
                OcenyUcznia = ocenyUcznia
            };
            
            return View(ViewModel);
        }

        public IActionResult Testy()
        {
            return RedirectToAction("Index", "Testy");
        }

        public IActionResult Wiadomosci()
        {
            return RedirectToAction("Index", "Wiadomosci");
        }
    }
}