using Brutus.Data;
using Microsoft.AspNetCore.Mvc;


namespace Brutus.Controllers
{
    public class WiadomosciController : Controller
    {
        private BrutusContext _context;

        public WiadomosciController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Wyslij()
        {

            return View();
        }
    }
}