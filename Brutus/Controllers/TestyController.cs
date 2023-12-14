using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;

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
            var testy = _context.Testy.ToList();
            return View(testy);
        }
        
    }
}
