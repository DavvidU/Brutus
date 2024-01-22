using Brutus.Models;

namespace Brutus.Services.Zestawienie
{
    public abstract class DodatkowaInformacja : Zestawienie
    {
        protected Zestawienie zestawienie;

        public DodatkowaInformacja(Zestawienie zestawienie) { this.zestawienie = zestawienie; }

        public override string GetDaneUcznia() { return zestawienie.GetDaneUcznia(); }
        public override string GetDanePrzedmiotu() { return zestawienie.GetDanePrzedmiotu(); }
        public override List<string> GetSformatowanaListaOcen() { return zestawienie.GetSformatowanaListaOcen(); }
        public override List<string> GetDodatkoweInformacje() { return zestawienie.GetDodatkoweInformacje(); }
        public override List<Ocena> GetOceny() { return zestawienie.GetOceny(); }
        public override int GetIdUcznia() { return zestawienie.GetIdUcznia(); }
        public override int GetIdPrzedmiotu() { return zestawienie.GetIdPrzedmiotu(); }


    }
}
