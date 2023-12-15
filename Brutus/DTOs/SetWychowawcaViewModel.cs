using Brutus.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class SetWychowawcaViewModel
    {
        public Klasa Klasa { get; set; }
        public SelectList Nauczyciele { get; set; }
        public int WybranyNauczycielID { get; set; }
    }
}
