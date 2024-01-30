using Brutus.Models;

namespace Brutus.Services.Zestawienie
{
    public class KomentarzeOcen : DodatkowaInformacja
    {
        public KomentarzeOcen(Zestawienie zestawienie) : base(zestawienie) { }
        public override string GetDaneUcznia() { return base.GetDaneUcznia(); }
        public override string GetDanePrzedmiotu() { return base.GetDanePrzedmiotu(); }
        public override List<string> GetSformatowanaListaOcen() 
        { 
            List<string> sformatowanaListaOcen = base.GetSformatowanaListaOcen();

            List<Ocena> listaOcen = GetOceny();

            // Dodaj komentarze ocen
            for (int i = 0; i < sformatowanaListaOcen.Count; ++i)
                sformatowanaListaOcen[i] = sformatowanaListaOcen[i] + " Komentarz: " + listaOcen[i].Komentarz;

            return sformatowanaListaOcen; 
        }
        public override List<string> GetDodatkoweInformacje() { return base.GetDodatkoweInformacje(); }
        public override List<Ocena> GetOceny() { return base.GetOceny(); }
        public override int GetIdUcznia() { return base.GetIdUcznia(); }
        public override int GetIdPrzedmiotu() { return base.GetIdPrzedmiotu(); }


    }
}
