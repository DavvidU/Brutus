using Brutus.Data;
using Brutus.Models;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Services.SortowanieUczniow
{
    public class SortowanieWzgledemOcen : ISortowanieUczniow
    {
        public List<Uczen> SortujUczniow(List<Uczen> uczniowieDoPosortowania, BrutusContext _context, int idPrzedmiotu)
        {
            List<Uczen> posortowaniUczniowie = new();

            Dictionary<Uczen, List<Ocena>> uczenOceny = new Dictionary<Uczen, List<Ocena>>();

            // Wypelnij slownik uczniami z ocenami z odpowiedniego przedmiotu
            foreach (var uczen in uczniowieDoPosortowania)
            {
                // pobierz oceny ucznia z odpowiedniego przedmiotu

                List<Ocena> ocenyUczniaZPrzedmiotu = _context.Oceny.Where(o => o.Uczen != null && o.Przedmiot != null &&
                    o.Uczen.ID_Ucznia == uczen.ID_Ucznia && o.Przedmiot.ID_Przedmiotu == idPrzedmiotu).ToList();

                uczenOceny.Add(uczen, ocenyUczniaZPrzedmiotu);
            }

            // Posortuj uczniow wedlug sredniej wazonej ich ocen z przedmiotu

            posortowaniUczniowie = PosortujSlownikIWyluskajUczniow(uczenOceny);

            return posortowaniUczniowie;
        }

        private List<Uczen> PosortujSlownikIWyluskajUczniow(Dictionary<Uczen, List<Ocena>> slownik)
        {
            List<Uczen> posortowaniUczniowie = new();

            // Słownik przechowujący ucznia i jego średnią ważoną
            Dictionary<Uczen, double> uczenSrednia = new Dictionary<Uczen, double>();

            foreach (var uczenOceny in slownik)
            {
                Uczen uczen = uczenOceny.Key;
                List<Ocena> oceny = uczenOceny.Value;

                // Obliczenie średniej ważonej
                double sumaWazona = oceny.Sum(o => o.Wartosc * o.Waga);
                double sumaWag = oceny.Sum(o => o.Waga);
                double sredniaWazona = sumaWazona / sumaWag;

                uczenSrednia.Add(uczen, sredniaWazona);
            }

            // Sortowanie słownika według średniej ważonej
            uczenSrednia = uczenSrednia.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            posortowaniUczniowie = uczenSrednia.Keys.ToList();

            return posortowaniUczniowie;
        }
    }
}
