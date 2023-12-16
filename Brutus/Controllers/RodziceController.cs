using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class RodziceController : Controller
    {
        private BrutusContext _context;
        public RodziceController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int idDodanegoKonta)
        {
            Rodzic rodzic = new Rodzic();
            rodzic.ID_Rodzica = idDodanegoKonta;

            _context.Rodzice.Add(rodzic);
            _context.SaveChanges();

            return RedirectToAction("Index", "Konta");
        }
    }
}
