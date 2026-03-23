class TaskSerivce : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly MyArray<TaskItem> _tasks;
    // private MyLinkedList<User> _users;

    public TaskSerivce(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
        // _users = new MyLinkedList<User>();

    }
    public IMyCollection<TaskItem> GetAllTasks() => _tasks;

    public void AddTask(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        Guid newId = GenerateGUID();
        var newTask = new TaskItem { ID = newId, Description = description, Completed = false };
        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }
    public void RemoveTask(Guid id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }

    public void ToggleTaskCompletion(Guid id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            task.Completed = !task.Completed;
            _repository.SaveTasks(_tasks);
        }
    }
    private Guid GenerateGUID() //jaro
    {
        return Guid.NewGuid();
    }

}