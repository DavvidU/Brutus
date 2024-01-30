using Brutus.Data;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Services.Command
{
    public static class IdTranslator
    {
        public static int TranslateToBusinessId(string userId, BrutusContext _context)
        {
            // znajdowanie konta użytkownika na podstawie jego identyfikatora
            Models.Konto userBusinessAccount = _context.Konta.FirstOrDefault(p => p.ApplicationUserId == userId);
            //jesli nie znalazlo konta
            if (userBusinessAccount == null) { return -1; }
            // zwroc identyfikator biznesowy znalezionego konta
            return userBusinessAccount.ID_Konta;
        }
    }
}
