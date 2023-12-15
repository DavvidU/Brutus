using Brutus.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class KlasaWithPrzedmioty
    {
        public Klasa Klasa { get; set; } 
        public List<Przedmiot> Przedmioty { get; set; }
        public SelectList WszystkiePrzedmioty { get; set; }
        public int WybranyPrzedmiotID { get; set; }
    }
}
