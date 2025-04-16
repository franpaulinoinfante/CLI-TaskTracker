using System;
using System.Text;

namespace CLI_TaskTracker.ConsoleApp.Models;

public class TaskJsonFile
{
    private const string WindowsPath = "D:\\Repos\\CLI-TaskTracker\\CLI-TaskTracker.ConsoleApp\\Data\\TaskJsonFile.json"; 
    private const string FedoraPath = "/home/franpaulino/Repos/CLI-TaskTracker/CLI-TaskTracker.ConsoleApp/Data/TaskJsonFile";

    private const string FilePath = TaskJsonFile.FedoraPath;
    private List<TaskItem> _taskItems;

    public TaskJsonFile(List<TaskItem> taskItems)
    {
        _taskItems = taskItems;

        if (!File.Exists(TaskJsonFile.FilePath))
        {
            File.WriteAllText(Path.Combine(TaskJsonFile.FilePath), string.Empty);
        }
    }

    public void AddTask(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            throw new ArgumentException("Debe crear una tarea");
        }

        string newTaskJson = ParseToJson(taskItem);

        if (new FileInfo(TaskJsonFile.FilePath).Length == 0)
        {
            File.WriteAllText(TaskJsonFile.FilePath, $"[{newTaskJson}]", Encoding.UTF8);
        }
        else
        {
            // Leer el contenido existente
            string jsonContent = File.ReadAllText(TaskJsonFile.FilePath);

            // encontrar el final del array (antes de ']')
            int indexOfClosingBracket = jsonContent.LastIndexOf(']');

            if (indexOfClosingBracket > 0)
            {
                // Insertar la nueva tarea JSON antes del corchete de cierre, con una coma si el array no estaba vac�o
                AddNewJsonLine(newTaskJson, jsonContent, indexOfClosingBracket);
            }
            else
            {
                // Si no se encuentra el corchete de cierre (JSON inv�lido), simplemente sobrescribir con un nuevo array
                File.WriteAllText(TaskJsonFile.FilePath, $"[{newTaskJson}]", Encoding.UTF8);
            }
        }
    }

    private static void AddNewJsonLine(string newTaskJson, string jsonContent, int indexOfClosingBracket)
    {
        string updateJson = jsonContent.Substring(0, indexOfClosingBracket);
        if (updateJson.LastIndexOf('{') > updateJson.LastIndexOf('['))
        {
            updateJson += "," + newTaskJson;
        }
        else
        {
            updateJson += newTaskJson;
        }

        updateJson += "]";
        File.WriteAllText(FilePath, updateJson, Encoding.UTF8);
    }

    private static string ParseToJson(TaskItem taskItem)
    {
        return $@"{{""Id"":{taskItem.Id},""Description"":""{EscapeJsonString(taskItem.Description)}"", ""Status"":""{EscapeJsonString(taskItem.Status)}"", ""CreatedAt"":""{taskItem.CreatedAt:yyyy-MM-ddTHH:mm:ssZ}"",""UpdatedAt"":""{taskItem.UpdatedAt:yyyy-MM-ddTHH:mm:ssZ}""}}";
    }

    private static string EscapeJsonString(string rawString)
    {

        return rawString.Replace("\"", "\\\"");
    }
}
