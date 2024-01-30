using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class UczenPanelController : Controller
    {
        private BrutusContext _context;
        public UczenPanelController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Uczen")]
        public IActionResult Oceny()
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idUcznia = invoker.Invoke();
            if (idUcznia == -1) { return NotFound(); }

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