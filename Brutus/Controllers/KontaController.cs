using Brutus.Data;
using Brutus.Models;
using Brutus.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Brutus.Controllers
{
    public class KontaController : Controller
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private BrutusContext _context;
        public KontaController(BrutusContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            Read();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string Imie, string Nazwisko, string Email, string SkrotHasla, int NrTelefonu, string TypKonta)
        {
            /* Konto - Identity */
            UserBuilder builder;

            switch (TypKonta)
            {
                case "Admin":
                    builder = new AdminBuilder(_context);
                    break;
                case "Uczen":
                    builder = new UczenBuilder(_context);
                    break;
                case "Nauczyciel":
                    builder = new NauczycielBuilder(_context);
                    break;
                case "Rodzic":
                    builder = new RodzicBuilder(_context);
                    break;
                default:
                    throw new ArgumentException("Nieznany typ konta");
            }

            Konto konto = new Konto();
            ApplicationUser user = builder.SetImie(Imie)
           .SetNazwisko(Nazwisko)
           .SetEmail(Email)
           .SetSkrotHasla(SkrotHasla)
           .SetNrTelefonu(NrTelefonu)
           .Build();

            var result = await _userManager.CreateAsync(user, SkrotHasla);
            if (result.Succeeded)
            {
                konto.ApplicationUserId = user.Id;
                builder.Save(user, _userManager);
                await _userManager.AddToRoleAsync(user, TypKonta);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(konto);
        }
        public IActionResult Read()
        {
            var przedmioty = _context.Konta.ToList();
            return View(przedmioty);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var konto = _context.Konta.FirstOrDefault(k => k.ID_Konta == id);
            if (konto == null)
                return NotFound();

            return View(konto);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Konto konto)
        {

            if (ModelState.IsValid)
            {
                var existingKonto = _context.Konta.FirstOrDefault(k => k.ID_Konta == konto.ID_Konta);

                if (existingKonto == null)
                    return NotFound();

                existingKonto.Imie = konto.Imie;
                existingKonto.Nazwisko = konto.Nazwisko;
                existingKonto.Email = konto.Email;
                existingKonto.SkrotHasla = konto.SkrotHasla;
                existingKonto.NrTelefonu = konto.NrTelefonu;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(konto);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var kontoDoUsuniecia = _context.Konta.FirstOrDefault(k => k.ID_Konta == id);
            if (kontoDoUsuniecia == null)
                return NotFound();

            _context.Konta.Remove(kontoDoUsuniecia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateForUser()
        {
            string userId = User.Identity.GetUserId();
            var konto = _context.Konta.FirstOrDefault(k => k.ApplicationUserId == userId);

            if (konto == null)
                return NotFound();

            return View(konto);
        }
        [HttpPost]
        public IActionResult UpdateForUser(Konto konto)
        {
            string userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                var existingKonto = _context.Konta.FirstOrDefault(k => k.ApplicationUserId == userId);

                if (existingKonto == null || existingKonto.ID_Konta != konto.ID_Konta)
                    return RedirectToAction("AccesDenied", "Home");

                existingKonto.Imie = konto.Imie;
                existingKonto.Nazwisko = konto.Nazwisko;
                existingKonto.Email = konto.Email;
                existingKonto.SkrotHasla = konto.SkrotHasla;
                existingKonto.NrTelefonu = konto.NrTelefonu;

                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(konto);
        }

    }
}
