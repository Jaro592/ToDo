public class TaskUser : IEquatable<TaskUser>
{
    public string TaskID { get; set; }
    public string UserID { get; set; }

    public bool Equals(TaskUser? other)
    {
        if (other == null) return false;
        return TaskID == other.TaskID && UserID == other.UserID;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TaskUser);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TaskID, UserID);
    }
    public override string ToString() //jaro
    {
        return $"TaskID: {TaskID}, UserID: {UserID}";
    }
}