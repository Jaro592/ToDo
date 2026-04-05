public class TaskUserService : ITaskUserService // Basel
{
    private ITaskUserRepository _taskUserRepository;
    private readonly ITaskRepository _taskRepository;

    private readonly IMyCollection<TaskUser> _userTasks; // jaro
    public TaskUserService(ITaskUserRepository taskUserRepository, ITaskRepository taskRepository)
    {
        _taskUserRepository = taskUserRepository;
        _taskRepository = taskRepository;

        _userTasks = _taskUserRepository.Load();
    }

    public bool Assign(string taskId, string userId) // Basel x changes by jaro
    {
        var iterator = _userTasks.GetIterator();
        while (iterator.HasNext())
        {
            var existing = iterator.Next();

            if (existing.UserID == userId && existing.TaskID == taskId)
            {
                return false;
            }

        }


        _userTasks.Add(new TaskUser
        {
            TaskID = taskId,
            UserID = userId
        });
        return true;
    }

    public void RemoveAllRelationsForTask(string taskId) // Jaro
    {
        // We maken een lijstje van alle relaties die we moeten verwijderen
        IMyCollection<TaskUser> toRemove = new MyLinkedList<TaskUser>();

        var iterator = _userTasks.GetIterator();
        while (iterator.HasNext())
        {
            var relation = iterator.Next();
            if (relation.TaskID == taskId)
            {
                toRemove.Add(relation);
            }
        }

        // Nu verwijderen we ze daadwerkelijk uit onze hoofdlijst
        var removeIterator = toRemove.GetIterator();
        while (removeIterator.HasNext())
        {
            _userTasks.Remove(removeIterator.Next());
        }
    }

    public IMyCollection<TaskItem> GetTasksForUser(string userId) // Jaro
    {
        var taskIds = _taskUserRepository.GetTasksForUser(_userTasks, userId);
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
        return _taskUserRepository.GetUsersForTask(_userTasks, taskId);
    }


    public void SaveAll() // jaro
    {
        _taskUserRepository.Save(_userTasks);
    }

}