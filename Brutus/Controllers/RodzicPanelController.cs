using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class RodzicPanelController : Controller
    {
        private BrutusContext _context;
        public RodzicPanelController(BrutusContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Uczniowie(int id)
        {
           
           var daneZUczniaIDaneZRodzica = (from konto in _context.Konta
               join uczen in _context.Uczniowie 
                   on konto.ID_Konta equals uczen.Konto.ID_Konta into xd 
               from hwdp in xd
               where hwdp.Rodzic.ID_Rodzica == id
                   select konto).ToList(); 

           var ViewModel = new RodzicWithUczniowie
           {
               idRodzica = id,
               kontaUczniowTegoRodzica = daneZUczniaIDaneZRodzica
           };
           
           return View(ViewModel);
        }
        
        [HttpGet]
        public IActionResult Oceny(int idUcznia)
        {
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
        
        public IActionResult Wiadomosci()
        {
            return RedirectToAction("Index", "Wiadomosci");
        }

        public IActionResult Oceny()
        {
            return RedirectToAction("Index", "Oceny");
        }
    }
}