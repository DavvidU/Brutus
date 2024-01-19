using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            return RedirectToAction("AccesDenied", "Home");
        }
        public IActionResult Login(string returnUrl)
        {
            return RedirectToAction("Login", "Home");
        }
    }
}
