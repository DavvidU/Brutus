using Brutus.Data;
using Brutus.Models;

namespace Brutus.Services.SortowanieUczniow
{
    public interface ISortowanieUczniow
    {
        List<Uczen> SortujUczniow(List<Uczen> uczniowieDoPosortowania, BrutusContext _context, int idPrzedmiotu);
    }
}
