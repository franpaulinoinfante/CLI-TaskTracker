namespace CLI_TaskTracker.ConsoleApp.Models;

public class TaskItem
{
    public TaskItem()
    {
        Status = "ToDo";
        CreatedAt = DateTime.Now;
        ModifyUpdatedAt();
    }

    public int Id { get; set; }
    public required string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public void MarkAsInProgress()
    {
        Status = "In Progress";
        ModifyUpdatedAt();
    }

    public void ModifyUpdatedAt() 
    {
        UpdatedAt = DateTime.Now;
    }

    public void MarkAsDone()
    {
        Status = "Done";
        ModifyUpdatedAt();
    }
}