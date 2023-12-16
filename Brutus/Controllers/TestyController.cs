using Brutus.Data;
using Brutus.DTOs;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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
        
        public IActionResult ShowForUczen()
        {
            IEnumerable<Test> testy = _context.Testy.ToList();
            return View(testy);
        }

        [HttpGet]
        public IActionResult RozwiazywanieTestu(int idTestu)
        {
            //_context.Pytania(p => p.Test.ID_Testu == idTestu).ToList()
            //Test test = _context.Testy.FirstOrDefault(t => t.ID_Testu == idTestu);
            List<Pytanie> pytaniaTestu = (from pytanie in _context.Pytania
                    where pytanie.Test != null && pytanie.Test.ID_Testu == idTestu
                        select pytanie).ToList();
            
            return View(pytaniaTestu);
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
        public IActionResult Details(int idTestu)
        {
            var test = _context.Testy.FirstOrDefault(t => t.ID_Testu == idTestu);
            if (test == null)
            {
                return NotFound();
            }
            var pytaniaDoTestu = (from pytanie in _context.Pytania
                                  where pytanie.Test.ID_Testu == idTestu
                                  select pytanie).ToList();

            var viewModel = new TestDetails
            {
                Test = test,
                Pytania = pytaniaDoTestu
            };
            return View(viewModel);
        }

    }
}
