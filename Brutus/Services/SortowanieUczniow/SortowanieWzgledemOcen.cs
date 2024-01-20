using Brutus.Data;
using Brutus.Models;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Services.SortowanieUczniow
{
    public class SortowanieWzgledemOcen : ISortowanieUczniow
    {
        public List<Uczen> SortujUczniow(List<Uczen> uczniowieDoPosortowania, BrutusContext _context)
        {
            List<Uczen> posortowaniUczniowie = new();

            Dictionary<Uczen, List<Ocena>> uczenOceny = new Dictionary<Uczen, List<Ocena>>();

            // Wypelnij slownik uczniami z ocenami z odpowiedniego przedmiotu
            foreach (var uczen in uczniowieDoPosortowania)
            {
                // pobierz rekordy uczen-ocena dla tego ucznia
                List<UczenOcena> uczenOcena = _context.UczniowieOceny.Include(uo => uo.Ocena).ThenInclude(p => p.Przedmiot).
                    Where(uo => uo.Uczen != null && uo.Ocena != null && uo.Uczen.ID_Ucznia == uczen.ID_Ucznia).ToList();
                // bycmoze bez "&& uo.Ocena != null"
                // Wydziel wszystkie oceny ucznia

                List<Ocena> wszystkieOcenyUcznia = new();

                foreach (var uczenOcenaRekord in uczenOcena)
                    wszystkieOcenyUcznia.Add(uczenOcenaRekord.Ocena);

                // Pozostaw tylko oceny z odpowiedniego przedmiotu

                List<Ocena> ocenyUczniaZPrzedmiotu = new();

                foreach (var ocenaUcznia in wszystkieOcenyUcznia)
                {
                    if (ocenaUcznia.Przedmiot.ID_Przedmiotu == 5)
                        ocenyUczniaZPrzedmiotu.Add(ocenaUcznia);
                }

                uczenOceny.Add(uczen, ocenyUczniaZPrzedmiotu);
            }

            // Posortuj uczniow wedlug sredniej wazonej ich ocen z przedmiotu



            return posortowaniUczniowie;
        }
    }
}
