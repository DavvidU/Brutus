using Brutus.Data;
using Brutus.Models;
using Microsoft.EntityFrameworkCore;

namespace Brutus.Services.Zestawienie
{
    public class TrendRozwojuNaTleKlasy : DodatkowaInformacja
    {
        private BrutusContext _context;
        public TrendRozwojuNaTleKlasy(Zestawienie zestawienie, BrutusContext _context) : base(zestawienie)
        { this._context = _context; }
        public override string GetDaneUcznia() { return base.GetDaneUcznia(); }
        public override string GetDanePrzedmiotu() { return base.GetDanePrzedmiotu(); }
        public override List<string> GetSformatowanaListaOcen() { return base.GetSformatowanaListaOcen(); }
        public override List<string> GetDodatkoweInformacje() 
        { 
            /* Dodatkowa informacja jest wpolczynnik kierunkowy regresji liniowej i jego stosunek do reszty klasy 
               Gdy wyznaczanie trendu nie ma sensu (brak ocen, brak uczniow) metoda zwraca informacje o tym */

            List<string> dodatkoweInformacje = base.GetDodatkoweInformacje();

            List<Ocena> ocenyUcznia = GetOceny();
            if (ocenyUcznia.Count <= 1)
            {
                dodatkoweInformacje.Add("Nie mozna wyznaczyc trendu rozwoju ucznia z powodu zbyt malej liczby ocen");
                return dodatkoweInformacje;
            }

            // Wyznacz trend ucznia

            double wspolczynnikKierunkowyUcznia = WyznaczWspolczynnikKierunkowyZOcen(ocenyUcznia);

            // Wyznacz trend reszty klasy

            double? sredniWspResztyKlasy = WyznaczSredniWspolczynnikResztyKlasy();
            if (sredniWspResztyKlasy == null)
            {
                dodatkoweInformacje.Add("Nie mozna wyznaczyc trendu rozwoju ucznia z powodu braku ocen innych uczniow");
                return dodatkoweInformacje;
            }

            // Dodaj trend rozwoju na tle klasy do listy "dodatkoweInformacje"

            string opisTrenduUcznia;
            string opisTrenduKlasy;
            string porownanieTrendow;

            if (wspolczynnikKierunkowyUcznia > 0)
                opisTrenduUcznia = "Uczeń wykazuje wzrostowy trend ocen";
            else if (wspolczynnikKierunkowyUcznia < 0)
                opisTrenduUcznia = "Uczeń wykazuje spadkowy trend ocen";
            else
                opisTrenduUcznia = "Oceny ucznia są stabilne";

            if (sredniWspResztyKlasy > 0)
                opisTrenduKlasy = "reszta klasy wykazuje wzrostowy trend ocen";
            else if (sredniWspResztyKlasy < 0)
                opisTrenduKlasy = "reszta klasy wykazuje spadkowy trend ocen";
            else
                opisTrenduKlasy = "oceny reszty klasy są stabilne";

            if (wspolczynnikKierunkowyUcznia == sredniWspResztyKlasy)
                porownanieTrendow = ", podobnie jak ";
            else
                porownanieTrendow = ", podczas gdy ";

            string trendRozwojuNaTleKlasy = opisTrenduUcznia + porownanieTrendow + opisTrenduKlasy;

            dodatkoweInformacje.Add(trendRozwojuNaTleKlasy);

            return dodatkoweInformacje;
        }
        public override List<Ocena> GetOceny() { return base.GetOceny(); }
        public override int GetIdUcznia() { return base.GetIdUcznia(); }
        public override int GetIdPrzedmiotu() { return base.GetIdPrzedmiotu(); }


        private double WyznaczWspolczynnikKierunkowyZOcen(List<Ocena> oceny) 
        {
            /* Jako zmienna niezalezna w regresji przyjeto indeks oceny
             * Na liscie sa one w kolejnosci zgodnej z data wystawienia */

            var sumaIndeksow = 0.0;
            var sumaWartosciOcen = 0.0;
            var sumaIloczynowIndeksWartosc = 0.0;
            var sumaKwadratowIndeksow = 0.0;

            for (int indeksOceny = 0; indeksOceny < oceny.Count; ++indeksOceny)
            {
                int indeksCzasowy = indeksOceny + 1; // Liczba porzadkowa oceny
                sumaIndeksow += indeksCzasowy;
                sumaWartosciOcen += oceny[indeksOceny].Wartosc;
                sumaIloczynowIndeksWartosc += indeksCzasowy * oceny[indeksOceny].Wartosc;
                sumaKwadratowIndeksow += indeksCzasowy * indeksCzasowy;
            }

            var wspolczynnikKierunkowy = (oceny.Count * sumaIloczynowIndeksWartosc - sumaIndeksow * sumaWartosciOcen) 
                / (oceny.Count * sumaKwadratowIndeksow - sumaIndeksow * sumaIndeksow);
            
            return wspolczynnikKierunkowy;
        }
        private double? WyznaczSredniWspolczynnikResztyKlasy()
        {
            /* Metoda zwraca null gdy nie ma sensu wyznaczanie wspolczynnika (brak ocen, brak uczniow) */

            List<double> trendyUczniow = new List<double>();

            Uczen? porownywanyUczen = _context.Uczniowie.Include(u => u.Klasa).
                                        FirstOrDefault(u => u.Klasa != null && u.ID_Ucznia == GetIdUcznia());
            if (porownywanyUczen == null) return null;

            int idKlasyPorownywanegoUcznia = porownywanyUczen.Klasa.ID_Klasy;

            // Pobierz reszte uczniow z klasy, do ktorej chodzi porownywany uczen

            List<Uczen> pozostaliUczniowie = _context.Uczniowie.Where(u => u.Klasa != null &&
                                    u.Klasa.ID_Klasy == idKlasyPorownywanegoUcznia).ToList();
            if (pozostaliUczniowie.Count == 0) return null;

            // Generowanie trendow dla kazdego ze znalezionych uczniow

            foreach (var uczen in pozostaliUczniowie)
            {
                // Pobierz oceny ucznia z odpowiedniego przedmiotu

                List<Ocena> ocenyUcznia = _context.Oceny.Where(o => o.Uczen != null && o.Przedmiot != null && 
                o.Uczen.ID_Ucznia == uczen.ID_Ucznia && o.Przedmiot.ID_Przedmiotu == GetIdPrzedmiotu()).ToList();

                if (ocenyUcznia.Count > 1)
                {
                    double trendUcznia = WyznaczWspolczynnikKierunkowyZOcen(ocenyUcznia);
                    trendyUczniow.Add(trendUcznia);
                }
            }

            // Jesli zaden z pozostalych uczniow nie mial ocen, metoda zwraca null

            double? sredniTrendKlasy = trendyUczniow.Any() ? trendyUczniow.Average() : null;
            
            return sredniTrendKlasy;
        }
    }
}
