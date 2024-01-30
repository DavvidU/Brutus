using Brutus.Data;
using Brutus.Models;
using Brutus.Services.Command;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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

            /* TU BYCMOZE TRZEBA UZYC .Include().ThenInclude().ToList() // OgloszeniaController:52 */
            var wiadomosciZOdbiorcami = _context.KontaWiadomosci.Where(kw => kw.Odbiorca.ID_Konta == idKonta);
            List<KontoWiadomosc> listaWiadomosciZOdbiorcami = wiadomosciZOdbiorcami.ToList();

            List<Wiadomosc> wiadomosciUzytkownika = new();

            foreach (var wiadomoscZOdbiarca in listaWiadomosciZOdbiorcami)
                wiadomosciUzytkownika.Add(wiadomoscZOdbiarca.Wiadomosc);

            return View(wiadomosciUzytkownika);
        }
        [HttpPost]
        public IActionResult Wyslij()
        {


            return View();
        }
    }
}