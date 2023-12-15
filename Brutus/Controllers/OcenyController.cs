using System.Linq;
using Brutus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OcenyController : Controller
{
    private readonly BrutusContext _context;

    public OcenyController(BrutusContext context)
    {
        _context = context;
    }

    public IActionResult Index(int idUcznia)
    {
        // Assuming you have a relationship between Uczen and Ocena
        var oceny = _context.Oceny
            .Include(o => o.Przedmiot) // Make sure to include Przedmiot
            //here(o => o.UczenID == idUcznia)
            .ToList();

        return View(oceny);
    }
}