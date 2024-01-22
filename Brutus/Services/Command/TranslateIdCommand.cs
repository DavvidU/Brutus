using Brutus.Data;
using Brutus.Models;
using Brutus.Services;
using Brutus.Services.Command;

public class TranslateIdCommand : ICommand
{
    private readonly BrutusContext _context;
    private readonly string _userId;

    public TranslateIdCommand( string userId, BrutusContext context)
    {
        _context = context;
        _userId = userId;
    }
    public int ProvideID()
    {
        return IdTranslator.TranslateToBusinessId(_userId, _context);
    }
}
