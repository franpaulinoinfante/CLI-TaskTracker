namespace CLI_TaskTracker.ConsoleApp.Models;

public class TaskItem
{
    public TaskItem()
    {
        Status = "ToDo";
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public int Id { get; init; }
    public required string Description { get; set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }

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