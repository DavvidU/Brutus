using Brutus.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class WiadomosciWithMozliwiAdresaci
    {
        public List<Wiadomosc> Wiadomosci { get; set; }
        public SelectList MozliwiAdresaci { get; set; }
        public int WybranyAdresatID { get; set; }
        public string? TrescWiadomosci { get; set; }
    }
}
