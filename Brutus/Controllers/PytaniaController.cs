using Brutus.Data;
using Brutus.Models;
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
        public IActionResult Create(int id, int liczbaZadan)
        {
            ViewBag.TestId = id;
            ViewBag.LiczbaZadan = liczbaZadan;
            return View(new List<Pytanie>(new Pytanie[liczbaZadan]));
        }

        [HttpPost]
        public IActionResult Create(int TestId, List<Pytanie> pytania)
        {
            if (ModelState.IsValid)
            {
                foreach (var pytanie in pytania)
                {
                    pytanie.Test = _context.Testy.FirstOrDefault(t => t.ID_Testu == TestId);
                    _context.Pytania.Add(pytanie);
                }
                _context.SaveChanges();
                return RedirectToAction("Show", "Testy");
            }
            ViewBag.TestId = TestId;
            ViewBag.LiczbaZadan = pytania.Count;
            return View(pytania);
        }
        [HttpGet]
        public IActionResult AddQuestions(int id, int liczbaZadan)
        {
            ViewBag.TestId = id;
            ViewBag.LiczbaZadan = liczbaZadan;
            var pytania = new List<Pytanie>(new Pytanie[liczbaZadan]);
            return View(pytania);
        }
    }
}
