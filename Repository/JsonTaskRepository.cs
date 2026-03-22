using System.IO;
using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath = filePath;

    public MyArray<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath)) return new MyArray<TaskItem>();

        string json = File.ReadAllText(_filePath);
        
        TaskItem[]? rawArray = JsonSerializer.Deserialize<TaskItem[]>(json);

        if (rawArray == null) return new MyArray<TaskItem>();

        MyArray<TaskItem> myCollection = new MyArray<TaskItem>(rawArray.Length);
        foreach (var item in rawArray)
        {
            myCollection.Add(item);
        }

        return myCollection;
    }
    public void SaveTasks(MyArray<TaskItem> tasks)
    {
        var cleanArray = tasks.Data; 
        string json = JsonSerializer.Serialize(cleanArray, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}