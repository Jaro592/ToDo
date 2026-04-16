using System.IO;
using System.Text.Json;
public class JsonTaskUserRepository : ITaskUserRepository
{
    private readonly string _filePath;
    private readonly IMyCollection<TaskUser> _relations;
    public JsonTaskUserRepository(string filePath, IMyCollection<TaskUser> relations)
    {
        _filePath = filePath;
        _relations = relations;
    }

    public IMyCollection<TaskUser> Load() // Basel
    {
        if (!File.Exists(_filePath)) return _relations;

        string json = File.ReadAllText(_filePath);
        TaskUser[]? rawArray = JsonSerializer.Deserialize<TaskUser[]>(json);

        if (rawArray == null) return _relations;

        IMyCollection<TaskUser> relations = _relations;

        foreach (var item in rawArray)
        {
            relations.Add(item);
        }

        return relations;
    }

    public void Save(IMyCollection<TaskUser> relations) //Basel
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
    public IMyCollection<string> GetUsersForTask(IMyCollection<TaskUser> relations, string taskID)// Basel
    {
        // var Relations = Load();
        IMyCollection<string> start = new MyLinkedList<string>();

        return relations.Reduce(start, (currentList, relation) =>
        {
            if (relation.TaskID == taskID)
                currentList.Add(relation.UserID);
            return currentList;
        });
    }

}