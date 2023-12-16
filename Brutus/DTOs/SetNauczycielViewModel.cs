using Brutus.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class SetNauczycielViewModel
    {
        public Przedmiot Przedmiot { get; set; }
        public SelectList Nauczyciele { get; set; }
        public int WybranyNauczycielID { get; set; }
    }
}
