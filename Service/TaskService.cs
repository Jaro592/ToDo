class TaskSerivce : Serialize,ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks;

    private readonly ITaskUserService _taskUserService;
    private MyLinkedList<User> _users;

    public TaskSerivce(ITaskRepository repository, ITaskUserService taskUserService)
    {
        _repository = repository;
        _taskUserService = taskUserService;
        _tasks = _repository.LoadTasks();
    }
    public IMyCollection<TaskItem> GetAllTasks() => _tasks;

    public void AddTask(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        string newId = NewSerializeString(); // jaro
        var newTask = new TaskItem { ID = newId, Description = description, Completed = false };
        _tasks.Add(newTask);
        //_repository.SaveTasks(_tasks);
    }
    public void RemoveTask(string id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            _taskUserService.RemoveAllRelationsForTask(id);  //jaro
            _tasks.Remove(task);

            //_repository.SaveTasks(_tasks);
        }
    }

    public void ToggleTaskCompletion(string id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            task.Completed = !task.Completed;
            //_repository.SaveTasks(_tasks);
        }
    }
    // private Guid GenerateGUID() //jaro
    // {
    //     return Guid.NewGuid();
    // }

    public void SaveAll() // jaro
    {
        _repository.SaveTasks(_tasks);
    }

}