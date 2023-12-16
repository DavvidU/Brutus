using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brutus.Controllers
{
    public class WiadomosciController : Controller
    {
        private readonly BrutusContext _context;

        public WiadomosciController(BrutusContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Wiadomosc> wiadomosciList = _context.Wiadomosci.ToList();
            return View(wiadomosciList);
        }

        public IActionResult Details(int id)
        {
            Wiadomosc wiadomosc = _context.Wiadomosci.FirstOrDefault(w => w.ID_Wiadomosci == id);
            if (wiadomosc == null)
            {
                return NotFound();
            }

            return View(wiadomosc);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Wiadomosc wiadomosc, IFormFile file)
        {
            // Validate and save the message
            if (ModelState.IsValid)
            {
                // Handle file attachment
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        //wiadomosc.Zalacznik = memoryStream.ToArray();
                    }
                }

                // Set other properties like ID_Nadawcy, Data, etc.
                //wiadomosc.ID_Nadawcy = 1; // Replace with the actual ID of the sender
                wiadomosc.Data = DateTime.Now;

                _context.Wiadomosci.Add(wiadomosc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wiadomosc);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Wiadomosc wiadomosc = _context.Wiadomosci.FirstOrDefault(w => w.ID_Wiadomosci == id);
            if (wiadomosc == null)
            {
                return NotFound();
            }

            _context.Wiadomosci.Remove(wiadomosc);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}