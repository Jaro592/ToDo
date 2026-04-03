using System.IO;
using System.Text.Json;
public class JsonTaskUserRepository : ITaskUserRepository
{
    private readonly string _filePath;
    public JsonTaskUserRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IMyCollection<TaskUser> Load()
    {
        if (!File.Exists(_filePath)) return new MyLinkedList<TaskUser>();

        string json = File.ReadAllText(_filePath);
        TaskUser[]? rawArray = JsonSerializer.Deserialize<TaskUser[]>(json);

        if (rawArray == null) return new MyLinkedList<TaskUser>();

        IMyCollection<TaskUser> relations = new MyLinkedList<TaskUser>();

        foreach (var item in rawArray)
        {
            relations.Add(item);
        }

        return relations;
    }

    public void Save(IMyCollection<TaskUser> relations)
    {
        TaskUser[] cleanArray = new TaskUser[relations.Count];

        int i = 0;
        var iterator = relations.GetIterator();

        while (iterator.HasNext())
        {
            cleanArray[i] = iterator.Next();
            i++;
        }

        string json = JsonSerializer.Serialize(cleanArray, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
    public IMyCollection<string> GetTasksForUser(IMyCollection<TaskUser> allRelations, string userId) // Jaro 
    {
        IMyCollection<string> startLijst = new MyLinkedList<string>();
        return allRelations.Reduce(startLijst, (huidigeLijst, relation) => 
        {
            if (relation.UserID == userId)
            {
                huidigeLijst.Add(relation.TaskID);
            }
            return huidigeLijst;
        });
    }
}