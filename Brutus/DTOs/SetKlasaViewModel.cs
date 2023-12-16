using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class SetKlasaViewModel
    {
        public int ID_Ucznia { get; set; }
        public string ImieUcznia { get; set; }
        public string NazwiskoUcznia { get; set; }
        public SelectList Klasy { get; set; }
        public int WybranaKlasaID { get; set; }
    }
}
