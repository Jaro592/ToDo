

class TaskSerivce : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly MyArray<TaskItem> _tasks;
    private MyLinkedList<User> _users;

    public TaskSerivce(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
        _users = new MyLinkedList<User>();

    }
    public IMyCollection<TaskItem> GetAllTasks() => _tasks;

    public void AddTask(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        int newId = _tasks.Count > 0 ? _tasks[_tasks.Count - 1].ID + 1 : 1;
        var newTask = new TaskItem { ID = newId, Description = description, Completed = false };
        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);

    }
    public void RemoveTask(int id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }

    public void ToggleTaskCompletion(int id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.ID.CompareTo(key));

        if (task != null)
        {
            task.Completed = !task.Completed;
            _repository.SaveTasks(_tasks);
        }
    }

    public void AddUser(string name)
    {
        var user = new User(name);
        _users.Add(user);
    }

    public User? FindUser(string name)
    {
        var i = _users.FindBy(name, (k, key) => k.Name.CompareTo(key));
        if (i != null)
        {
            return i;
        }
        return default!;
    }



    public void AssignTaskToUser(int taskId, string userName)
    {
        var task = _tasks.FindBy(taskId, (t, key) => t.ID.CompareTo(key));
        var user = FindUser(userName);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            Console.ReadKey();
            return;
        }
        if (task != null && user != null)
        {
            user.Tasks.Add(task);
            task.AssignedUser = userName;
            _repository.SaveTasks(_tasks);
        }
    }



}