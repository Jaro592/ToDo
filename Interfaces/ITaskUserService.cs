public interface ITaskUserService
{
    void Assign(int taskId, int userId);

    // IMyCollection<TaskItem> GetTasksForUser(int userId);

    // IMyCollection<User> GetUsersForTask(int taskId);
}