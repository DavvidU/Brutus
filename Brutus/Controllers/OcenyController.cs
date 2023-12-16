using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Controllers
{
    public class OcenyController : Controller
    {
        private BrutusContext _context;

        public OcenyController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Ocena ocena)
        {

            if (ModelState.IsValid)
            {
                _context.Oceny.Add(ocena);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(ocena);
        }

    }
}