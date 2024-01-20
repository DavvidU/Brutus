using Brutus.Data;
using Brutus.Models;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Services.SortowanieUczniow
{
    public class SortowanieAlfabetyczne : ISortowanieUczniow
    {
        public List<Uczen> SortujUczniow(List<Uczen> uczniowieDoPosortowania, BrutusContext _context, int idPrzedmiotu)
        {
            List<Uczen> posortowaniUczniowie = new();

            List<Konto> kontaUczniow = new();

            foreach (Uczen uczen in uczniowieDoPosortowania)
            {
                Konto kontoUcznia = _context.Konta.First(k => k.ID_Konta == uczen.ID_Ucznia);

                if (kontoUcznia != null)
                    kontaUczniow.Add(kontoUcznia);
            }

            kontaUczniow = kontaUczniow
            .OrderBy(konto => konto.Nazwisko)
            .ThenBy(konto => konto.Imie)
            .ToList();

            foreach (Konto kontoUcznia in kontaUczniow)
            {
                Uczen uczen = _context.Uczniowie.First(u => u.ID_Ucznia == kontoUcznia.ID_Konta);

                if (uczen != null)
                    posortowaniUczniowie.Add(uczen);
            }

            return posortowaniUczniowie;
        }
    }
}
