using Brutus.Data;
using Brutus.Models;
using Brutus.Services.Command;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Controllers
{
    public class OgloszeniaController : Controller
    {
        private BrutusContext _context;

        public OgloszeniaController(BrutusContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ogloszenia = _context.Ogloszenia.Include(o => o.Nauczyciel).ThenInclude(n => n.Konto).ToList();
            return View(ogloszenia);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Nauczyciel")]
        public IActionResult Create(Ogloszenie ogloszenie)
        {
            string userId = User.Identity.GetUserId();

            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int idNauczyciela = invoker.Invoke();

            if (ModelState.IsValid)
            {
                ogloszenie.Nauczyciel = _context.Nauczyciele.FirstOrDefault(n => n.ID_Nauczyciela == idNauczyciela);
                _context.Ogloszenia.Add(ogloszenie);;
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(ogloszenie);
        }

        
        public IActionResult Aktualnosci()
        {
            var ogloszenia = _context.Ogloszenia.Include(o => o.Nauczyciel).ThenInclude(n => n.Konto).ToList();
            
            return View(ogloszenia);
        }

        [Authorize(Roles = "Nauczyciel")]
        public IActionResult AktualnosciEdit()
        {
           // pobranie identyfikatora użytkownika
            string userId = User.Identity.GetUserId();

            // utworzenie konkretnego polecenia do przetłumaczenia identyfikatora uzytkownika na identyfikator biznesowy
            var command = new TranslateIdCommand(userId, _context);
            // utworzenie obiektu Invoker który bedzie wywolywac polecenie
            var invoker = new Invoker();
            // przekazanie komendy do wywoływacza
            invoker.SetCommand(command);
            // wywolanie komendy i otrzymanie identyfikatora biznesowego nauczyciela
            int idNauczyciela = invoker.Invoke();

            if (idNauczyciela == -1) { return NotFound(); }

            var ogloszeniaNauczyciela = _context.Ogloszenia.Where(o => o.Nauczyciel.ID_Nauczyciela == idNauczyciela);
            List<Ogloszenie> listaOgloszenNauczyciela = ogloszeniaNauczyciela.ToList();

            return View(listaOgloszenNauczyciela);
        }

        [HttpPost]
        public IActionResult AktualnosciEdit(int id)
        {
            var ogloszenieDoUsuniecia = _context.Ogloszenia.FirstOrDefault(o => o.ID_Ogloszenia == id);
            if (ogloszenieDoUsuniecia == null)
            {
                return NotFound();
            }

            _context.Ogloszenia.Remove(ogloszenieDoUsuniecia);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var ogloszenie = _context.Ogloszenia.FirstOrDefault(o => o.ID_Ogloszenia == id);
            if (ogloszenie == null)
            {
                return NotFound();
            }
            return View(ogloszenie);
        }

        [HttpPost]
        public IActionResult Update(Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                var existingOgloszenie = _context.Ogloszenia.FirstOrDefault(o => o.ID_Ogloszenia == ogloszenie.ID_Ogloszenia);
                if (existingOgloszenie == null)
                {
                    return NotFound();
                }
                existingOgloszenie.Tresc = ogloszenie.Tresc;
                existingOgloszenie.Nauczyciel = ogloszenie.Nauczyciel;

                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(ogloszenie);
        }
    }
}
