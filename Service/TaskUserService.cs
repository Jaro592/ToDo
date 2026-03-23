public class TaskUserService : ITaskUserService
{
    private ITaskUserRepository _taskUserRepository;
    public TaskUserService(ITaskUserRepository taskUserRepository) =>
    _taskUserRepository = taskUserRepository;

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
}