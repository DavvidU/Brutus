﻿using Brutus.Data;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Controllers
{
    public class NauczycielPanelController : Controller
    {
        private BrutusContext _context;

        public NauczycielPanelController(BrutusContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult PokazPrzedmioty()
        {
            return RedirectToAction("ReadForNauczyciel", "Przedmioty");
        }
        public IActionResult Ogłoszenia()
        {
            return RedirectToAction("Index", "Ogłoszenia");
        }
        public IActionResult Wiadomosc()
        {
            return RedirectToAction("Index", "Wiadomosci");
        }
    }
}