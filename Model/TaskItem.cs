public class TaskItem : IEquatable<TaskItem>
{
    public int ID { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }
    public string? AssignedUser { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TaskItem);
    }

    public bool Equals(TaskItem? other)
    {
        return other != null && ID == other.ID;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    public override string ToString()
    {
        return $"[{ID}] {Description} - {(Completed ? "Done" : "in progress")}";
    }
}