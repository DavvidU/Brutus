using Brutus.Models;

namespace Brutus.Services.ZestawienieUcznia
{
    public class TrendRozwojuNaTleKlasy : DodatkowaInformacja
    {
        private List<Uczen> uczniowieWKlasie;
        public TrendRozwojuNaTleKlasy(Zestawienie zestawienie, List<Uczen> uczniowieWKlasie) : base(zestawienie)
        { this.uczniowieWKlasie = uczniowieWKlasie; }
        public new string GetDaneUcznia() { return base.GetDaneUcznia(); }
        public new string GetDanePrzedmiotu() { return base.GetDanePrzedmiotu(); }
        public new List<string> GetSformatowanaListaOcen() { return base.GetSformatowanaListaOcen(); }
        public new List<string> GetDodatkoweInformacje() 
        { 
            /* Dodatkowa informacja jest wpolczynnik kierunkowy regresji liniowej */

            List<string> dodatkoweInformacje = base.GetDodatkoweInformacje();

            List<Ocena> ocenyUcznia = GetOceny();

            // Wyznacz trend ucznia

            double wsp = WyznaczWspolczynnikKierunkowyZOcen(ocenyUcznia);

            // Wyznacz trend reszty klasy

            double sredniWspResztyKlasy = 1;

            // Stosunek trendu ucznia do sredniego trendu klasy

            double stosunekTrendow = wsp / sredniWspResztyKlasy;

            // Dodaj trend rozwoju na tle klasy do listy "dodatkoweInformacje"

            string trendRozwojuNaTleKlasy = "Trend rozwoju ucznia: " + wsp.ToString() +
                ". Średni trend reszty klasy: " + sredniWspResztyKlasy.ToString() + ". Uczen rozwija sie w tempie " +
                stosunekTrendow.ToString() + " w porównaniu do reszty klasy";

            dodatkoweInformacje.Add(trendRozwojuNaTleKlasy);

            return dodatkoweInformacje;
        }
        public new List<Ocena> GetOceny() { return base.GetOceny(); }
        public new int GetIdUcznia() { return base.GetIdUcznia(); }

        private double WyznaczWspolczynnikKierunkowyZOcen(List<Ocena> oceny) 
        {
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
        private double WyznaczSredniWspolczynnik
    }
}
