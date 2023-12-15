using Brutus.Models;

namespace Brutus.DTOs
{
    public class UczenWithKlasaWithRodzic
    {
        public int ID_Ucznia { get; set; }
        public string ImieUcznia { get; set; }
        public string NazwiskoUcznia { get; set; }

        public int? ID_Klasy { get; set; }
        public int? NumerRoku { get; set; }
        public string LiteraKlasy { get; set; }

        public int? ID_Rodzica { get; set; }
        public string ImieRodzica { get; set; }
        public string NazwiskoRodzica { get; set; }
    }
}
