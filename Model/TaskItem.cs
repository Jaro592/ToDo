public class TaskItem : IEquatable<TaskItem>, IComparable<TaskItem> // jaro
{
    public string ID { get; set; } // jaro
    public required string Description { get; set; }
    public bool Completed { get; set; }
    // public string? AssignedUser { get; set; }
    public int Priority { get; set; } = 1; // 1 = Low, 2 = Medium, 3 = High

    public string PriorityText => Priority switch //akif
    {
        1 => "Low",
        2 => "Medium",
        3 => "High",
        _ => "Low"
    };

    public override bool Equals(object? obj) // jaro
    {
        return Equals(obj as TaskItem);
    }

    public int CompareTo(TaskItem? other)
    {
        if (other == null) return 1;
        return string.Compare(ID, other.ID, StringComparison.OrdinalIgnoreCase);
    }

    public bool Equals(TaskItem? other) // jaro
    {
        return other != null && ID == other.ID;
    }

    public override int GetHashCode() // jaro
    {
        return ID.GetHashCode();
    }

    public override string ToString() // jaro
    {
        return $"{Description} - {(Completed ? "Done" : "in progress")}";
    }
}