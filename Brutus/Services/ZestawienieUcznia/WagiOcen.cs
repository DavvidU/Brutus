using Brutus.Models;

namespace Brutus.Services.ZestawienieUcznia
{
    public class WagiOcen : DodatkowaInformacja
    {
        public WagiOcen(Zestawienie zestawienie) : base(zestawienie) { }
        public new string GetDaneUcznia() { return base.GetDaneUcznia(); }
        public new string GetDanePrzedmiotu() { return base.GetDanePrzedmiotu(); }
        public new List<string> GetSformatowanaListaOcen()
        {
            List<string> sformatowanaListaOcen = base.GetSformatowanaListaOcen();

            List<Ocena> listaOcen = GetOceny();

            // Dodaj wagi ocen
            for (int i = 0; i < sformatowanaListaOcen.Count; ++i)
                sformatowanaListaOcen[i] = sformatowanaListaOcen[i] + " Waga: " + listaOcen[i].Waga.ToString();

            return sformatowanaListaOcen;
        }
        public new List<string> GetDodatkoweInformacje() { return base.GetDodatkoweInformacje(); }
        public new List<Ocena> GetOceny() { return base.GetOceny(); }
        public new int GetIdUcznia() { return base.GetIdUcznia(); }

    }
}
