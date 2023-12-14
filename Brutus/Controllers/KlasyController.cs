using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class KlasyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
