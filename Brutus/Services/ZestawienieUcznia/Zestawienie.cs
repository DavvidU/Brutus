using Brutus.Models;

namespace Brutus.Services.ZestawienieUcznia
{
    public class Zestawienie
    {
        //private Uczen uczen;
        private int idUcznia;
        private string imieUcznia;
        private string nazwiskoUcznia;
        private int idPrzedmiotu;
        private string nazwaPrzedmiotu;
        private List<string> sformatowanaListaOcen;
        private string dodatkowaTresc;

        public string GetDaneUcznia()
        {
            string daneUcznia = idUcznia.ToString() + imieUcznia + " " + nazwiskoUcznia;
            return daneUcznia;
        }
        public string GetDanePrzedmiotu()
        {
            string danePrzedmiotu = idPrzedmiotu.ToString() + " " + nazwaPrzedmiotu; 
            return danePrzedmiotu;
        }
        public string GetSformatowanaListaOcen()
        {

        }
    }
}
