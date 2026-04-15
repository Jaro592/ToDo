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

    public void AddTask(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        string newId = NewSerializeString(); // jaro
        var newTask = new TaskItem { ID = newId, Description = description, Completed = false };
        _tasks.Add(newTask);
    }
    public void RemoveTask(string id) // Remove Dependency by Basel
    {
        var task = FindTask(id);
        if (task == null) return;

        var it = _tasks.GetIterator();
        while (it.HasNext())
        {
            var t = it.Next();
            t.DependencyIds.Remove(id);
        }


        _taskUserService.RemoveAllRelationsForTask(id);  //jaro
        _tasks.Remove(task);


    }

    public bool ToggleTaskCompletion(string id) // changes by Basel
    {
        var task = FindTask(id);

        if (task is null) return false;

        if (!task.Completed)
        {
            if (!CanComplete(task))
                return false;
        }

        task.Completed = !task.Completed;
        return true;
        //_repository.SaveTasks(_tasks);

    }

    public void SaveAll() // jaro
    {
        _repository.SaveTasks(_tasks);
    }
    public TaskItem? FindTask(string id) // Basel
    {
        return _tasks.FindBy(id, (task, key) => task.ID.CompareTo(key));
    }
    public bool AddDependency(string taskId, string dependencyId) //Basel
    {
        if (taskId.Equals(dependencyId)) return false;

        var task = FindTask(taskId);
        var dependecy = FindTask(dependencyId);

        if (task is null || dependecy is null) return false;

        var it = task.DependencyIds.GetIterator();
        while (it.HasNext())
        {
            if (it.Next().Equals(dependencyId)) return false;
        }

        task.DependencyIds.Add(dependencyId);

        return true;
    }

    public bool CanComplete(TaskItem task)
    {
        var it = task.DependencyIds.GetIterator();

        while (it.HasNext())
        {
            var depId = it.Next();
            var depTask = FindTask(depId);
            if (depTask == null || !depTask.Completed)
            {
                return false;
            }
        }

        return true;
    }

}