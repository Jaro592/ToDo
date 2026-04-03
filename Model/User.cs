public class User : IEquatable<User> // Basel
{
    public string UserID { get; set; } // jaro
    public string Name { get; set; }

    public User(string name)
    {
        Name = name;

    }

    public bool Equals(User? other) // Basel
    {
        if (other == null) return false;
        return Name == other.Name;
    }

    public override bool Equals(object? obj) // Basel
    {
        return Equals(obj as User);
    }

    public override int GetHashCode() // Basel
    {
        return Name.GetHashCode();
    }

    public override string ToString() // Basel
    {
        return Name;
    }
}