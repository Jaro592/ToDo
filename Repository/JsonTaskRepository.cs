using System.IO;
using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath = filePath;

    public IMyCollection<TaskItem> LoadTasks()
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

    public TaskItem GetById(string taskId) // Jaro
    {
        var allTasks = this.LoadTasks();
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