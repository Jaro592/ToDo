using System.IO;
using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly IMyCollection<User> _users;
    public JsonUserRepository(string filePath, IMyCollection<User> users)
    {
        _filePath = filePath;
        _users = users;
    }

    public IMyCollection<User> LoadUsers()
    {
        if (!File.Exists(_filePath)) return _users;
        string json = File.ReadAllText(_filePath);
        User[]? rawUsers = JsonSerializer.Deserialize<User[]>(json);
        if (rawUsers == null) return _users;
        IMyCollection<User> users = _users;
        foreach (var user in rawUsers)
        {
            users.Add(user);
        }
        return users;
    }

    public void SaveUsers(IMyCollection<User> users)
    {
        User[] cleanArray = new User[users.Count];
        int i = 0;
        var iterator = users.GetIterator();
        while (iterator.HasNext())
        {
            cleanArray[i] = iterator.Next();
            i++;
        }
        string json = JsonSerializer.Serialize(cleanArray, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
    public User? GetById(string userId) // Basel
    {
        var allUsers = this.LoadUsers();
        var iterator = allUsers.GetIterator();

        while (iterator.HasNext())
        {
            var currentUser = iterator.Next();

            if (currentUser.UserID == userId)
            {
                return currentUser;
            }
        }

        return null;
    }

}