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
           // Nie wiem co robie
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