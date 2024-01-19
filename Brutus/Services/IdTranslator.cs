using Brutus.Data;
using Microsoft.AspNetCore.Mvc;

namespace Brutus.Services
{
    public static class IdTranslator
    {
        public static int TranslateToBusinessId(string userId, BrutusContext _context)
        {
            Models.Konto userBusinessAccount = _context.Konta.FirstOrDefault(p => p.ApplicationUserId == userId);

            if(userBusinessAccount == null) { return -1; }

            return userBusinessAccount.ID_Konta;
        }
    }
}
