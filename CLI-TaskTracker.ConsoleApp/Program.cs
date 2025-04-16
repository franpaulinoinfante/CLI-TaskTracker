using CLI_TaskTracker.ConsoleApp.Models;

new TaskTracker().Manage();

internal class TaskTracker
{
    private readonly List<TaskItem> _taskItems;
    private int _id;

    private TaskJsonFile _taskJsonFile;

    public TaskTracker()
    {
        _taskItems = new List<TaskItem>();
        _id = default;

        _taskJsonFile = new();
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
                result = commandParser.Parser(input.ToLower());
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
                    _id = _taskJsonFile.GetMaxTaskIdManual() + 1;
                    TaskItem taskITem = new () { Id = _id, Description = arguments[0]};
                    _taskJsonFile.AddTask(taskITem);
                    _taskItems.Add(taskITem);
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