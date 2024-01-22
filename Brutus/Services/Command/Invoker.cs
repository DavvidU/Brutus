public class Invoker
{
    private ICommand _command;

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public int Invoke()
    {
        return _command.ProvideID();
    }
}   