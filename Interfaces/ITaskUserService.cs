public interface ITaskUserService
{
    void Assign(Guid taskId, Guid userId);

    // IMyCollection<TaskItem> GetTasksForUser(int userId);

    // IMyCollection<User> GetUsersForTask(int taskId);
}