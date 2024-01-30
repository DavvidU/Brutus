using Brutus.Data;
using Brutus.Services.Command;

namespace Brutus.Services
{
    public static class AccesVerification
    {
        public static bool Verify(int requestedId, string userId, BrutusContext _context)
        {
            var command = new TranslateIdCommand(userId, _context);
            var invoker = new Invoker();
            invoker.SetCommand(command);

            int userBusinessId = invoker.Invoke();

            return requestedId == userBusinessId;
        }
    }
}
