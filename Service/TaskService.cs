class TaskSerivce : Serialize, ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks;

    private readonly ITaskUserService _taskUserService;
    //private MyLinkedList<User> _users;

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
    public bool AddDependency(string taskId, string dependencyId)
    {
        if (taskId.Equals(dependencyId)) return false;

        var task = FindTask(taskId);
        var dependency = FindTask(dependencyId);

        if (task is null || dependency is null) return false;

        var it = task.DependencyIds.GetIterator();
        while (it.HasNext())
        {
            if (it.Next().Equals(dependencyId)) return false;
        }

        if (WouldCreateCycle(taskId, dependencyId)) return false;

        task.DependencyIds.Add(dependencyId);
        return true;
    }

    private bool WouldCreateCycle(string taskId, string dependencyId)
    {
        var visited = new MyArray<string>();
        var stack = new MyLinkedList<string>();
        stack.AddFirst(dependencyId);

        while (stack.Count > 0)
        {
            stack.Reset();
            string current = stack.Next();
            stack.Remove(current);

            if (current.Equals(taskId)) return true;

            if (visited.FindBy(current, (s, k) => s.CompareTo(k)) != null) continue;
            visited.Add(current);

            var currentTask = FindTask(current);
            if (currentTask is null) continue;

            var iter = currentTask.DependencyIds.GetIterator();
            while (iter.HasNext())
            {
                string nextId = iter.Next();
                if (visited.FindBy(nextId, (s, k) => s.CompareTo(k)) == null)
                    stack.AddFirst(nextId);
            }
        }

        return false;
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