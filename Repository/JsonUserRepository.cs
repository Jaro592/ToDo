using System.IO;
using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    public JsonUserRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IMyCollection<User> LoadUsers()
    {
        if (!File.Exists(_filePath)) return new MyLinkedList<User>();
        string json = File.ReadAllText(_filePath);
        User[]? rawUsers = JsonSerializer.Deserialize<User[]>(json);
        if (rawUsers == null) return new MyLinkedList<User>();
        IMyCollection<User> users = new MyLinkedList<User>();
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


}