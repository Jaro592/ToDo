public class TaskUserService : ITaskUserService // Basel
{
    private ITaskUserRepository _taskUserRepository;
    private readonly ITaskRepository _taskRepository;
    // private IUserRepository _userRepository;
    public TaskUserService(ITaskUserRepository taskUserRepository, ITaskRepository taskRepository)
    {
        _taskUserRepository = taskUserRepository;
        _taskRepository = taskRepository;
        // _userRepository = userRepository;
    }

    public void Assign(string taskId, string userId) // Basel x changes by jaro
    {
        var userTask = _taskUserRepository.Load();
        var relation = new TaskUser
        {
            TaskID = taskId,
            UserID = userId
        };

        userTask.Add(relation);
        _taskUserRepository.Save(userTask);
    }

    public IMyCollection<TaskItem> GetTasksForUser(string userId) // Jaro
    {
        var taskIds = _taskUserRepository.GetTasksForUser(userId);
        IMyCollection<TaskItem> tasksForUser = new MyLinkedList<TaskItem>();

        var iterator = taskIds.GetIterator();
        return taskIds.Reduce(tasksForUser, (huidigeLijst, taskId) =>
        {
            TaskItem realTask = _taskRepository.GetById(taskId);

            if (realTask != null)
            {
                huidigeLijst.Add(realTask);
            }
            return huidigeLijst;
        });
    }




    public IMyCollection<string> GetUsersForTask(string taskId) // Basel
    {
        return _taskUserRepository.GetUsersForTask(taskId);
    }

    // public IMyCollection<User> GetUsersForTask(string taskId)//Basel
    // {
    //     var usersIds = _taskUserRepository.GetUsersForTask(taskId);
    //     IMyCollection<User> usersForTask = new MyLinkedList<User>(); ;

    //     var iterator = usersIds.GetIterator();
    //     return usersIds.Reduce(usersForTask, (currentList, userId) =>
    //     {
    //         User user = _userRepository.GetById(userId);

    //         if (user != null)
    //         {
    //             currentList.Add(user);
    //         }
    //         return currentList;
    //     });

    // }
}