using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Brutus.Controllers
{
    public class KontaController : Controller
    {
        private BrutusContext _context;
        public KontaController(BrutusContext context)
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

        //string Imie, string Nazwisko, string Email, string SkrotHasla, int NrTelefonu, string TypKonta
        /*Konto konto = new Konto();
        konto.Imie = Imie;
        konto.Nazwisko = Nazwisko;
        konto.Email = Email;
        konto.SkrotHasla = SkrotHasla;
        konto.NrTelefonu = NrTelefonu; */
        [HttpPost]
        public IActionResult Create(string Imie, string Nazwisko, string Email, string SkrotHasla, int NrTelefonu, string TypKonta)
        {
            //if (ModelState.IsValid)
            //{
            Konto konto = new Konto();
            konto.Imie = Imie;
            konto.Nazwisko = Nazwisko;
            konto.Email = Email;
            konto.SkrotHasla = SkrotHasla;
            konto.NrTelefonu = NrTelefonu;
            string typTworzonegoKonta = TypKonta;
            int idTworzonegoKonta;

            if (ModelState.IsValid)
            {
                _context.Konta.Add(konto);
//                _context.SaveChanges();
                
            /*    idTworzonegoKonta = _context.Konta.OrderByDescending(x => x.ID_Konta).FirstOrDefault().ID_Konta;
                //  idTworzonegoKonta = konto.ID_Konta;
                
                if(typTworzonegoKonta == "Admin")
                    return RedirectToAction("Create", "Admini", new { idDodanegoKonta = idTworzonegoKonta });
                if (typTworzonegoKonta == "Uczen")
                    return RedirectToAction("Create", "Uczniowie", new { idDodanegoKonta = idTworzonegoKonta });
                if (typTworzonegoKonta == "Rodzic")
                    return RedirectToAction("Create", "Rodzice", new { idDodanegoKonta = idTworzonegoKonta });
                if (typTworzonegoKonta == "Nauczyciel")
                    return RedirectToAction("Create", "Nauczyciele", new { idDodanegoKonta = idTworzonegoKonta });
            }*/
            }

                return View(konto);
            
        }
        public IActionResult Read()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
