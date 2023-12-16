using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brutus.DTOs
{
    public class SetRodzicViewModel
    {
        public int ID_Ucznia { get; set; }
        public string ImieUcznia { get; set; }
        public string NazwiskoUcznia { get; set; }
        public SelectList Rodzice { get; set; }
        public int WybranyRodzicID { get; set; }
    }
}
