using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class AdminiController : Controller
    {
        private BrutusContext _context;
        public AdminiController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int idDodanegoKonta)
        {
            Admin admin = new Admin();
            admin.ID_Admina = idDodanegoKonta;

            _context.Admini.Add(admin);
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Konta");
        }
    }
}
