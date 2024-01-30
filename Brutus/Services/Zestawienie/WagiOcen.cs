using Brutus.Models;

namespace Brutus.Services.Zestawienie
{
    public class WagiOcen : DodatkowaInformacja
    {
        public WagiOcen(Zestawienie zestawienie) : base(zestawienie) { }
        public override string GetDaneUcznia() { return base.GetDaneUcznia(); }
        public override string GetDanePrzedmiotu() { return base.GetDanePrzedmiotu(); }
        public override List<string> GetSformatowanaListaOcen()
        {
            List<string> sformatowanaListaOcen = base.GetSformatowanaListaOcen();

            List<Ocena> listaOcen = GetOceny();

            // Dodaj wagi ocen
            for (int i = 0; i < sformatowanaListaOcen.Count; ++i)
                sformatowanaListaOcen[i] = sformatowanaListaOcen[i] + " Waga: " + listaOcen[i].Waga.ToString();

            return sformatowanaListaOcen;
        }
        public override List<string> GetDodatkoweInformacje() { return base.GetDodatkoweInformacje(); }
        public override List<Ocena> GetOceny() { return base.GetOceny(); }
        public override int GetIdUcznia() { return base.GetIdUcznia(); }
        public override int GetIdPrzedmiotu() { return base.GetIdPrzedmiotu(); }


    }
}
