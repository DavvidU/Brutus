using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 

namespace Brutus.Controllers
{
    public class TestyController : Controller
    {
        private BrutusContext _context;

        public TestyController(BrutusContext context)
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
        public IActionResult Create(Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Testy.Add(test);
                _context.SaveChanges();
                return RedirectToAction("Show", "Testy");
            }
            return View(test);
        }
        public IActionResult Show()
        {
            IEnumerable<Test> testy = _context.Testy.ToList();
            return View(testy);
        }
        [HttpGet]
        public IActionResult RedirectToAddQuestions(int id)
        {
            var test = _context.Testy.FirstOrDefault(t => t.ID_Testu == id);
            if (test == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "Pytania", new { id = id, liczbaZadan = test.LiczbaZadan });
        }
        public IActionResult Details(int id)
        {
            var test = _context.Testy.FirstOrDefault(t => t.ID_Testu == id);
            if (test == null)
            {
                return NotFound();
            }

            var pytania = _context.Pytania.Where(p => p.ID_Testu == id).ToList();

            // Przekazanie testu i pytań do widoku za pomocą ViewBag
            ViewBag.Test = test;
            ViewBag.Pytania = pytania;

            return View();
        }



    }
}
