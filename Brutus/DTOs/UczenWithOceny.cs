using Brutus.Models;

namespace Brutus.DTOs
{
    public class UczenWithOceny
    {
        public Konto KontoUcznia { get; set; }
        public List<Ocena> OcenyUcznia { get; set; }
    }
};
