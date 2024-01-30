using Brutus.Models;

namespace Brutus.Services.Zestawienie
{
    public class ZestawienieUcznia : Zestawienie
    {
        private int idUcznia;
        private string imieUcznia;
        private string nazwiskoUcznia;
        private int idPrzedmiotu;
        private string nazwaPrzedmiotu;
        private List<Ocena> listaOcen;
        private List<string> dodatkoweInformacje = new();

        public ZestawienieUcznia(int idUcznia, string imieUcznia, string nazwiskoUcznia, int idPrzedmiotu, 
            string nazwaPrzedmiotu, List<Ocena> listaOcen)
        {
            this.idUcznia = idUcznia;
            this.imieUcznia = imieUcznia;
            this.nazwiskoUcznia = nazwiskoUcznia;
            this.idPrzedmiotu = idPrzedmiotu;
            this.nazwaPrzedmiotu = nazwaPrzedmiotu;
            this.listaOcen = listaOcen;
        }

        public override string GetDaneUcznia()
        {
            string daneUcznia = idUcznia.ToString() + " " + imieUcznia + " " + nazwiskoUcznia;
            return daneUcznia;
        }
        public override string GetDanePrzedmiotu()
        {
            string danePrzedmiotu = idPrzedmiotu.ToString() + " " + nazwaPrzedmiotu; 
            return danePrzedmiotu;
        }
        public override List<string> GetSformatowanaListaOcen()
        {
            List<string> sformatowanaListaOcen = new List<string>();
            
            foreach (Ocena ocena in listaOcen) 
                sformatowanaListaOcen.Add(ocena.Wartosc.ToString());

            return sformatowanaListaOcen;
        }
        public override List<string> GetDodatkoweInformacje() { return dodatkoweInformacje; }
        public override List<Ocena> GetOceny() { return listaOcen; }
        public override int GetIdUcznia() { return idUcznia; }
        public override int GetIdPrzedmiotu() { return idPrzedmiotu; }
    }
}
