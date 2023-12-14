using Brutus.Data;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class PytaniaController : Controller
    {
        private BrutusContext _context;

        public PytaniaController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
