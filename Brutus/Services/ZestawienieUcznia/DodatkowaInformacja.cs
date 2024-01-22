using Brutus.Models;

namespace Brutus.Services.ZestawienieUcznia
{
    public abstract class DodatkowaInformacja : Zestawienie
    {
        protected Zestawienie zestawienie;

        public DodatkowaInformacja(Zestawienie zestawienie) { this.zestawienie = zestawienie; }

        public new string GetDaneUcznia() { return zestawienie.GetDaneUcznia(); }
        public new string GetDanePrzedmiotu() { return zestawienie.GetDanePrzedmiotu(); }
        public new List<string> GetSformatowanaListaOcen() { return zestawienie.GetSformatowanaListaOcen(); }
        public new List<string> GetDodatkoweInformacje() { return zestawienie.GetDodatkoweInformacje(); }
        public new List<Ocena> GetOceny() { return zestawienie.GetOceny(); }
        public new int GetIdUcznia() { return zestawienie.GetIdUcznia(); }

    }
}
