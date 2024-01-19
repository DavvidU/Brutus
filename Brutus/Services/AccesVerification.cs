using Brutus.Data;

namespace Brutus.Services
{
    public static class AccesVerification
    {
        public static bool Verify(int requestedId, string userId, BrutusContext _context)
        {
            int userBusinessId = IdTranslator.TranslateToBusinessId(userId, _context);
            
            return requestedId == userBusinessId;
        }
    }
}
