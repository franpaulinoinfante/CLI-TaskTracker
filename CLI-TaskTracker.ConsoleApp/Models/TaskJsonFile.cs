using System;

namespace CLI_TaskTracker.ConsoleApp.Models;

public class TaskJsonFile
{
    private const string FilePath = "/home/franpaulino/Repos/CLI-TaskTracker/CLI-TaskTracker.ConsoleApp/Data/TaskJsonFile";
    private List<TaskItem> _taskItems;

    public TaskJsonFile(List<TaskItem> taskItems)
    {
        _taskItems = taskItems;
    }

    public void AddTask(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            throw new ArgumentException("Debe crear una tarea");
        }

        // Proceso para guardar en un archivo Json 

        _taskItems.Add(taskItem);
    }
}
