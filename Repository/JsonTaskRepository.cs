using System.IO;
using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly IMyCollection<TaskItem> _tasks;
    public JsonTaskRepository(string filePath, IMyCollection<TaskItem> tasks)
    {
        _filePath = filePath;
        _tasks = tasks;
    } 

    public IMyCollection<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath)) return _tasks;

        string json = File.ReadAllText(_filePath);

        TaskItem[]? rawArray = JsonSerializer.Deserialize<TaskItem[]>(json);

        if (rawArray == null) return new MyArray<TaskItem>();

        IMyCollection<TaskItem> myCollection = _tasks;
        foreach (var item in rawArray)
        {
            myCollection.Add(item);
        }

        return myCollection;
    }
    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        TaskItem[] taskItems = new TaskItem[tasks.Count];
        IMyIterator<TaskItem> iterator = tasks.GetIterator();
        int index = 0;
        while (iterator.HasNext())
        {
            taskItems[index++] = iterator.Next();
        }

        string json = JsonSerializer.Serialize(taskItems, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public TaskItem? GetById(string taskId) // Jaro
    {
        var allTasks = LoadTasks();
        var iterator = allTasks.GetIterator();
        while (iterator.HasNext())
        {
            var currentTask = iterator.Next();
            if (currentTask.ID == taskId)
            {
                return currentTask;
            }
        }
        return null;
    }
}