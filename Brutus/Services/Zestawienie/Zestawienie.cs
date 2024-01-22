using Brutus.Models;

namespace Brutus.Services.Zestawienie
{
    public abstract class Zestawienie
    {
        public abstract string GetDaneUcznia();
        public abstract string GetDanePrzedmiotu();
        public abstract List<string> GetSformatowanaListaOcen();
        public abstract List<string> GetDodatkoweInformacje();
        public abstract List<Ocena> GetOceny();
        public abstract int GetIdUcznia();
        public abstract int GetIdPrzedmiotu();
    }
}
