

class TaskSerivce : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks;
    private MyLinkedList<User> _users;
    private int maxId = 0;

    public TaskSerivce(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
        _users = new MyLinkedList<User>();
        maxId = GetInitialMaxId();
    }
    public IMyCollection<TaskItem> GetAllTasks() => _tasks;

    private int GetInitialMaxId()
    {
        int max = 0;
        var it = _tasks.GetIterator();
        it.Reset();
        while (it.HasNext())
        {
            int currentId = it.Next().ID;
            if (currentId > max) max = currentId;
        }
        return max;
    }

    public void AddTask(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        var it = _tasks.GetIterator();
        it.Reset();
        while (it.HasNext())
        {
            if (it.Next().Description.Equals(description, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Fout: Deze taak bestaat al!");
                return; 
            }
        }

        maxId++; 
        
        var newTask = new TaskItem 
        { 
            ID = maxId, 
            Description = description, 
            Completed = false 
        };

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