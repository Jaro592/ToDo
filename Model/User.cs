public class User : IEquatable<User>
{
    public string Name { get; set; }
    public IMyCollection<TaskItem> Tasks { get; set; }

    public User(string name)
    {
        Name = name;
        Tasks = new MyArray<TaskItem>();
    }

    public bool Equals(User? other)
    {
        if (other == null) return false;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as User);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}