public class CommandInvoker
{
    private readonly IList<ICommand> _history = new List<ICommand>();

    public void Invoke(ICommand command)
    {
        command.Execute();
        _history.Add(command);
    }
}
