public class User : IEquatable<User>,  IComparable<User> // Basel
{
    public string UserID { get; set; } // jaro
    public string Name { get; set; }

    public User(string userID, string name)
    {
        UserID = userID;
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

    public int CompareTo(User? other) => UserID.CompareTo(other?.UserID);

    public int CompareTo(object? obj) => CompareTo(obj as User);

    public override string ToString() // Basel
    {
        return Name;
    }
}