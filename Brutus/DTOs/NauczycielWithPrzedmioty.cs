using Brutus.Models;

namespace Brutus.DTOs
{
    public class NauczycielWithPrzedmioty
    {
        public Konto KontoNauczyciela { get; set; }
        public List<Przedmiot> PrzedmiotyNauczyciela { get; set; }
    }
}
