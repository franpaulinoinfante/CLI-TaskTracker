namespace CLI_TaskTracker.ConsoleApp.Models;

internal class Task
{
    private int _id;

    public int Id 
    { 
        get => _id;
        set
        {
            if (_id > 0)
            {
                throw new InvalidOperationException("Ya posee un id");
            }

            Id = value;
        }
    }
    public required string Description { get; set; }
    public required string Status { get; set; }
    public required DateTime CreateAt { get; set; }
    public required string UpdateAt { get; set; }
}