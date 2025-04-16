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
        CommandParser commandParser = new();
        DisplayHeaderMessages();
        (string command, string[]? parameters) result = (result.command = string.Empty, default);
        do
        {
            Console.Write("Task Tracker CLI > ");
            string input = Console.ReadLine() ?? string.Empty;

            try
            {
                result = commandParser.Parser(input);
                ProcessCommand(result.command, result.parameters!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! {ex.Message}");
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

    private static void Display(TaskItem taskItem)
    {
        Console.WriteLine("Id\tDescripción\t\tEstado\tCreada\t\t\tUltima Actualización");
        Console.WriteLine($"{taskItem.Id}\t{taskItem.Description}\t{taskItem.Status}\t{taskItem.CreatedAt}\t{taskItem.UpdatedAt}");
    }

    private static void DisplayHeaderMessages()
    {
        Console.Title = "Task Tracker CLI";
        Console.WriteLine("Bienvenido a Task Tracker CLI");
        CommandParser.DisplayCommands();
    }
}