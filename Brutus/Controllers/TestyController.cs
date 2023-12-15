using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Brutus.Controllers
{
    public class TestyController : Controller
    {
        private readonly BrutusContext _context;

        public TestyController(BrutusContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve the logged-in user's ID or any necessary information
            // to filter tests based on the user (assuming you have authentication implemented)

            // Example: int loggedInUserId = GetLoggedInUserId(); 

            // Fetch tests for the logged-in user
            var tests = _context.Testy
                //.Where(t => t.UczenId == loggedInUserId) // Adjust this based on your data model
                .ToList();

            return View(tests);
        }
    }
}