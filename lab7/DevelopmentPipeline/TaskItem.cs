namespace lab7;

using System;

public class TaskItem : ITaskItem
{
    public int Id { get; }
    public DateTime CreatedAt { get; }

    public TaskItem(int id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
    }

    public override string ToString() => $"Task-{Id}";
}
