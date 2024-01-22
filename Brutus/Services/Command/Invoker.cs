public class Invoker
{
    private ICommand _command;

    // metoda pozwalająca na ustawienie konkretnego polecenia która ma być wykonana
    // metoda jest wywoływana przez klienta który konfiguruje Invoker przed jego użyciem
    public void SetCommand(ICommand command)
    {
        _command = command;
    }
    // metoda Invoke wykonuje polecenie centralna część wzorca
    // gdzie wywoływacz (invoker) wyzwala działanie polecenia bez wiedzy co robi ProvideID
    public int Invoke()
    {
        return _command.ProvideID();
    }
}   