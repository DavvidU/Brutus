using Brutus.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class WiadomoscZAdresatami
    {
        public Wiadomosc Wiadomosc { get; set; }
        public List<string> Adresaci { get; set; }
    }
    public class WiadomosciWithMozliwiAdresaci
    {
        public List<Wiadomosc> WiadomosciOdebrane { get; set; }
        public List<WiadomoscZAdresatami> WiadomosciWyslane { get; set; }
        public SelectList MozliwiAdresaci { get; set; }
        public int WybranyAdresatID { get; set; }
        public string? TrescWiadomosci { get; set; }
    }
}
