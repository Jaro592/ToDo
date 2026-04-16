class TaskSerivce : Serialize, ITaskService
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

    public void AddTask(string? description, int priority = 1)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        if (priority < 1 || priority > 3) priority = 1;
        string newId = NewSerializeString(); // jaro
        var newTask = new TaskItem { ID = newId, Description = description, Completed = false, Priority = priority };
        _tasks.Add(newTask);
    }

    public void SetTaskPriority(string id, int priority) //akif
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));
        if (task == null) return;
        if (priority < 1 || priority > 3) return;
        task.Priority = priority;
    }

    public void RemoveTask(string id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            _taskUserService.RemoveAllRelationsForTask(id);  //jaro
            _tasks.Remove(task);

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

    public void SaveAll() // jaro
    {
        _repository.SaveTasks(_tasks);
    }

}