using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CLI_TaskTracker.ConsoleApp.Models;

public class TaskJsonFile
{
    private const string WindowsPath = "D:\\Repos\\CLI-TaskTracker\\CLI-TaskTracker.ConsoleApp\\Data\\TaskJsonFile.json";
    private const string FedoraPath = "/home/franpaulino/Repos/CLI-TaskTracker/CLI-TaskTracker.ConsoleApp/Data/TaskJsonFile.json";

    private const string FilePath = TaskJsonFile.FedoraPath;

    public TaskJsonFile()
    {
        TaskJsonFile.CreateTaskJsonFile();
    }

    private static void CreateTaskJsonFile()
    {
        if (!File.Exists(TaskJsonFile.FilePath))
        {
            File.WriteAllText(Path.Combine(TaskJsonFile.FilePath), "TaskJsonFile.json");
        }
    }

    public int GetMaxTaskId()
    {
        int maxId = 0;

        if (!File.Exists(TaskJsonFile.FilePath))
        {
            return maxId; // Si el archivo no existe, el máximo ID es 0
        }

        string jsonContent = File.ReadAllText(TaskJsonFile.FilePath);

        // Intentar encontrar todos los objetos JSON dentro del array
        MatchCollection matches = Regex.Matches(jsonContent, @"\{[^}]*\}");

        foreach (Match match in matches)
        {
            string jsonObject = match.Value;

            // Extraer el Id usando una expresión regular básica
            Match idMatch = Regex.Match(jsonObject, @"""Id"":\s*(\d+)");
            if (idMatch.Success && int.TryParse(idMatch.Groups[1].Value, out int currentId))
            {
                maxId = Math.Max(maxId, currentId);
            }
        }

        return maxId;
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
        string updateJson = jsonContent[..indexOfClosingBracket];
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

    private string ParseToJson(TaskItem taskItem) => $@"{{""Id"": {taskItem.Id}, ""Description"": ""{EscapeJsonString(taskItem.Description)}"", ""Status"": ""{EscapeJsonString(taskItem.Status)}"", ""CreatedAt"": ""{taskItem.CreatedAt:yyyy-MM-ddTHH:mm:ssZ}"",""UpdatedAt"": ""{taskItem.UpdatedAt:yyyy-MM-ddTHH:mm:ssZ}""}}";

    private string EscapeJsonString(string rawString)
    {
        return rawString.Replace("\"", string.Empty).Trim();
    }

    // private TaskItem? MapTaskJsonToTaskItem(string jsonContent, int taskItemId)
    // {
    //     TaskItem? taskItem = null;
    //     // Intentar encontrar todos los objetos JSON dentro del array
    //     MatchCollection matches = Regex.Matches(jsonContent, @"\{[^}]*\}");

    //     foreach (Match match in matches)
    //     {
    //         string jsonObject = match.Value;
    //         Match idMatch = Regex.Match(jsonObject, @"""Id"":\s*(\d+)");
    //         if (idMatch.Success && int.TryParse(idMatch.Groups[1].Value, out int currentId) && currentId == taskItemId)
    //         {
    //             //TryParseTaskItem(jsonObject, out TaskItem? task);
    //             //taskItem = task;
    //         }
    //     }

    //     return taskItem; // No se encontró ningún TaskItem con el Id especificado
    // }

    private TaskItem? ParseToTaskItem(string jsonObject)
    {
        // Extraer el Id
        Match idMatch = Regex.Match(jsonObject, @"""Id"":\s*(\d+)");
        if (!idMatch.Success || !int.TryParse(idMatch.Groups[1].Value, out int id))
        {
            return null;
        }

        // Extraer la Descripción
        string? description = ExtractPropertyValue(jsonObject, "Description");
        if (description == null)
        {
            return null;
        }

        // Extraer el Status
        string? status = ExtractPropertyValue(jsonObject, "Status");
        if (status == null)
        {
            status = "ToDo"; // Valor por defecto si no se encuentra
        }

        // Extraer CreatedAt
        DateTime createdAt = ExtractDateTimePropertyValue(jsonObject, "CreatedAt");

        // Extraer UpdatedAt
        DateTime updatedAt = ExtractDateTimePropertyValue(jsonObject, "UpdatedAt");

        return new TaskItem
        {
            Id = id,
            Description = description,
            Status = status,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    private string? ExtractPropertyValue(string jsonObject, string propertyName)
    {
        string pattern = $@"""{propertyName.Trim()}"":\s*""((?:\\.|[^""])*)""";
        Match match = Regex.Match(jsonObject, pattern);
        return match.Success ? match.Groups[1].Value.Replace("\\\"", "\"") : null;
    }

    private DateTime ExtractDateTimePropertyValue(string jsonObject, string propertyName)
    {
        string pattern = $@"""{propertyName}"":\s*""([^""]*)""";
        Match match = Regex.Match(jsonObject, pattern);
        if (match.Success && DateTime.TryParseExact(match.Groups[1].Value, "yyyy-MM-ddTHH:mm:ssZ", null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime parsedDateTime))
        {
            return parsedDateTime;
        }

        return default;
    }

    public List<TaskItem> GetAllTasksFromFile()
    {
        List<TaskItem> tasks = new List<TaskItem>();

        if (!File.Exists(TaskJsonFile.FedoraPath))
        {
            throw new InvalidOperationException("El archivo de tareas no existe.");
        }

        string jsonContent = File.ReadAllText(TaskJsonFile.FedoraPath);

        // Intentar encontrar todos los objetos JSON dentro del array
        MatchCollection matches = Regex.Matches(jsonContent, @"\{[^}]*\}");

        foreach (Match match in matches)
        {
            string jsonObject = match.Value;

            var taskItem = ParseToTaskItem(jsonObject);
            if (taskItem == null)
            {
                throw new InvalidCastException("No se pudo convertir de Json a Objeto");
            }

            tasks.Add(taskItem);
        }

        return tasks;
    }

    public void Update(TaskItem taskItemToUpdate)
    {
        if (!File.Exists(TaskJsonFile.FedoraPath))
        {
            throw new InvalidOperationException("El archivo de tareas no existe.");
        }

        string jsonContent = File.ReadAllText(TaskJsonFile.FedoraPath);

        string pattern = $@"{{\s*""Id"":\s*{taskItemToUpdate.Id},(.*?)}}";
        Match match = Regex.Match(jsonContent, pattern, RegexOptions.Singleline);

        string updateTask = ParseToJson(taskItemToUpdate);

        if (match.Success)
        {
            string oldTask = match.Value;
            jsonContent = jsonContent.Replace(oldTask, updateTask);
            File.WriteAllText(TaskJsonFile.FedoraPath, jsonContent);
        }
    }

    public TaskItem? FindTaskItemById(int taskItemId)
    {
        if (!File.Exists(TaskJsonFile.FilePath))
        {
            return null;
        }

        // obtener el texto json
        string jsonContent = File.ReadAllText(TaskJsonFile.FilePath);

        // Intentar encontrar todos los objetos JSON dentro del array
        MatchCollection matches = Regex.Matches(jsonContent, @"\{[^}]*\}");
        foreach (Match match in matches)
        {
            string jsonObject = match.Value;
            Match idMatch = Regex.Match(jsonObject, @"""Id"":\s*(\d+)");
            if (idMatch.Success && int.TryParse(idMatch.Groups[1].Value, out int currentId) && currentId == taskItemId)
            {
                return ParseToTaskItem(jsonObject);
            }
        }

        return null;
    }

    internal void Delete(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Debe ingresar un id entero positivo");
        }

        string jsonContent = File.ReadAllText(TaskJsonFile.FilePath);

        string pattern = $@"{{\s*""Id"":\s*{id},.*?}}";
        Match match = Regex.Match(jsonContent, pattern, RegexOptions.Singleline);
        if (!match.Success)
        {
            throw new InvalidOperationException($"Tarea con el id {id}, no encontrada");
        }

        string taskToRemove = match.Value;

        // Intentar eliminar también la coma antes o después si existe
        string cleanedJson = jsonContent.Replace($",{taskToRemove}", ""); // si está en medio
        cleanedJson = cleanedJson.Replace($"{taskToRemove},", ""); // si está al principio
        cleanedJson = cleanedJson.Replace(taskToRemove, "");       // si está sola

        // Limpiar el arreglo para que no queden comas mal puestas
        cleanedJson = Regex.Replace(cleanedJson, @"\[\s*,", "[");
        cleanedJson = Regex.Replace(cleanedJson, @",\s*\]", "]");

        File.WriteAllText(TaskJsonFile.FilePath, cleanedJson);
    }
}