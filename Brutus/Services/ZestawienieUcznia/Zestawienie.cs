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
        private List<Ocena> listaOcen;
        private List<string> dodatkoweInformacje;

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
        public List<string> GetSformatowanaListaOcen()
        {
            List<string> sformatowanaListaOcen = new List<string>();
            
            foreach (Ocena ocena in listaOcen) 
                sformatowanaListaOcen.Add(ocena.Wartosc.ToString());

            return sformatowanaListaOcen;
        }
        public List<string> GetDodatkoweInformacje()
        {
            dodatkoweInformacje = new List<string>();
            return dodatkoweInformacje;
        }
        public List<Ocena> GetOceny() { return listaOcen; }
        public int GetIdUcznia() { return idUcznia; }
    }
}
