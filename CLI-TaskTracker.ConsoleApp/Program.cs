using CLI_TaskTracker.ConsoleApp.Models;

new TaskTracker().Manage();

internal class TaskTracker
{
    private TaskJsonFile _taskJsonFile;

    public TaskTracker()
    {
        _taskJsonFile = new();
    }

    internal void Manage()
    {
        DisplayHeaderMessages();
        
        CommandParser commandParser = new();
        commandParser.DisplayCommands();
        (string command, string[]? parameters) arguments = (arguments.command = string.Empty, default);
        do
        {
            string commandLine = ReadCommandLine();

            try
            {
                arguments = commandParser.Parser(commandLine.ToLower());
                ProcessCommand(arguments.command, arguments.parameters!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! {ex.Message}");
            }
        } while (!arguments.command.Equals("salir", StringComparison.CurrentCultureIgnoreCase));
    }

    private static void DisplayHeaderMessages()
    {
        Console.Title = "Task Tracker CLI";
        Console.WriteLine("Bienvenido a Task Tracker CLI");
    }

    private string ReadCommandLine()
    {
        Console.Write("Task Tracker CLI > ");
        string input = Console.ReadLine() ?? string.Empty;
        return input;
    }

    private void ProcessCommand(string command, string[] parameters)
    {
        int id;
        switch (command)
        {
            case "add":
                TaskItem taskITem = new() { Description = parameters[0] };
                _taskJsonFile.AddTask(taskITem);
                Display("Tarea agregada");
                break;
            case "update":
                int.TryParse(parameters[0], out id);
                TaskItem taskItemToUpdate = _taskJsonFile.FindTaskItemById(id)!;
                taskItemToUpdate.Description = parameters[1];
                _taskJsonFile.Update(taskItemToUpdate);
                Display("Tarea actualizada");
                break;
            case "delete" :
                int.TryParse(parameters[0], out id);                
                _taskJsonFile.Delete(id);
                Display("Tarea eliminada");
                break;
            case "mark-in-progress" :
                int.TryParse(parameters[0], out id);
                TaskItem taskToMarkInProgress = _taskJsonFile.FindTaskItemById(id)!;
                taskToMarkInProgress.Status = "En Progreso";
                _taskJsonFile.Update(taskToMarkInProgress);
                Display("nuevo estado: 'en progreso' ");
                break;
            case "mark-done" :
                int.TryParse(parameters[0], out id);
                TaskItem taskItemToMarkDone = _taskJsonFile.FindTaskItemById(id)!;
                taskItemToMarkDone.Status = "Realizada";
                _taskJsonFile.Update(taskItemToMarkDone);
                Display("nuevo estado: 'realizada' ");
                break;
            case "list" :
                string? parameter = string.IsNullOrWhiteSpace(parameters[0]) ? "" : parameters[0];
                var tasks = _taskJsonFile.GetTaskItems(parameter);
                Console.WriteLine("Id\tDescripción\t\tEstado\tCreada\t\t\tUltima Actualización");
                foreach (var task in tasks)
                {
                    Display(task);
                }
                break;
            default:
                Console.WriteLine("Gracias por usar nuestra applicacin");
                break;
        }
    }

    private void Display(string message)
    {
        Console.WriteLine(message);
    }

    private void Display(TaskItem taskItem)
    {
        Console.WriteLine($"{taskItem.Id}\t{taskItem.Description}\t{taskItem.Status}\t{taskItem.CreatedAt}\t{taskItem.UpdatedAt}");
    }
}