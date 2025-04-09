public class CommandInvoker
{
    private readonly IList<ICommand> _history = new List<ICommand>();

    public void Invoke(ICommand command)
    {
        command.Execute();
        _history.Add(command);
    }

    // За бажанням можна додати метод для скасування останньої команди (Undo)
    public void Undo()
    {
        // Реалізацію збереження попереднього стану чи виклик методу Undo() для конкретної команди потрібно реалізувати
    }
}
