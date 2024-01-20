using Brutus.Data;
using Brutus.Models;
using Brutus.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Brutus.Controllers
{
    public class KontaController : Controller
    {
        private BrutusContext _context;
        private UserManager<ApplicationUser> _userManager; /* Identity */
        public KontaController(BrutusContext context, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Read();
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        //string Imie, string Nazwisko, string Email, string SkrotHasla, int NrTelefonu, string TypKonta
        /*Konto konto = new Konto();
        konto.Imie = Imie;
        konto.Nazwisko = Nazwisko;
        konto.Email = Email;
        konto.SkrotHasla = SkrotHasla;
        konto.NrTelefonu = NrTelefonu; */
        [HttpPost]
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
            /* Konto - biznesowa logika 
           
            konto.Imie = Imie;
            konto.Nazwisko = Nazwisko;
            konto.Email = Email;
            konto.SkrotHasla = SkrotHasla;
            konto.NrTelefonu = NrTelefonu;
            konto.ApplicationUserId = user.Id;
            string typTworzonegoKonta = TypKonta;*/
            int idTworzonegoKonta;


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
                    idTworzonegoKonta = _context.Konta.OrderByDescending(x => x.ID_Konta).FirstOrDefault().ID_Konta;
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
        public IActionResult Update(int id)
        {
            var konto = _context.Konta.FirstOrDefault(k => k.ID_Konta == id);
            if (konto == null)
                return NotFound();

            return View(konto);
        }
        [HttpPost]
        public IActionResult Update(Konto konto)
        {
            if(ModelState.IsValid)
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
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var kontoDoUsuniecia = _context.Konta.FirstOrDefault(k => k.ID_Konta == id);
            if (kontoDoUsuniecia == null)
                return NotFound();

            _context.Konta.Remove(kontoDoUsuniecia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
