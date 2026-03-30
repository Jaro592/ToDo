public class TaskUserService : ITaskUserService
{
    private ITaskUserRepository _taskUserRepository;
    private readonly ITaskRepository _taskRepository;
    public TaskUserService(ITaskUserRepository taskUserRepository, ITaskRepository taskRepository)
    {
        _taskUserRepository = taskUserRepository;
        _taskRepository = taskRepository;
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
        while (iterator.HasNext())
        {
            string taskId = iterator.Next();
            TaskItem realTask = _taskRepository.GetById(taskId);
            if (realTask != null)
            {
                tasksForUser.Add(realTask);
            }
        }
        return tasksForUser;
    }
}