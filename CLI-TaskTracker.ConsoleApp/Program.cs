using CLI_TaskTracker.ConsoleApp.Models;

var path = Directory.GetCurrentDirectory();

new TaskTracker().Manage();

internal class TaskTracker
{
    private readonly List<TaskItem> _taskItems;
    private TaskJsonFile _taskJsonFile;

    public TaskTracker()
    {
        _taskItems = [];
        _taskJsonFile = new(_taskItems);
    }

    internal void Manage()
    {
        CommandParser commandParser = new CommandParser();
        DisplayHeaderMessages(commandParser);
        (string command, string[]? parameters) result;
        do
        {
            Console.Write("Task Tracker CLI > ");
            string input = Console.ReadLine() ?? string.Empty;

            result = commandParser.Parser(input);
            if (string.IsNullOrWhiteSpace(result.command))
            {
                Console.WriteLine("Error, comando introducido no valido");
            }
            else
            {
                ProcessCommand(result.command, result.parameters!);
            }        
        } while (!result.command.Equals("salir", StringComparison.CurrentCultureIgnoreCase));
    }

    private void ProcessCommand(string command, string[] arguments)
    {
        try
        {
            switch (command)
            {
                case "add":
                    _taskJsonFile.AddTask(new TaskItem{ Description = arguments[0]});
                    Console.WriteLine("Tarea agregada");
                    break;
                default:
                    Console.WriteLine("Gracias por usar nuestra applicacin");
                    break;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void DisplayHeaderMessages(CommandParser commandParser)
    {
        Console.Title = "Task Tracker CLI";
        Console.WriteLine("Bienvenido a Task Tracker CLI");
        commandParser.DisplayCommands();
    }
}