using Brutus.Models;

namespace Brutus.DTOs
{
    public class DodawanieOcenyViewModel
    {
        public int Wartosc { get; set; }
        public int Waga { get; set; }
        public string Komentarz { get; set; }
        public Uczen Uczen { get; set; }
        public Nauczyciel Nauczyciel { get; set; }
        public Przedmiot Przedmiot { get; set; }

        public DodawanieOcenyViewModel(Uczen uczen, Nauczyciel nauczyciel, Przedmiot przedmiot)
        {
            Uczen = uczen;
            Nauczyciel = nauczyciel;
            Przedmiot = przedmiot;
        }
        public DodawanieOcenyViewModel() { }
    }
}
