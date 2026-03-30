public interface ITaskUserService
{
    void Assign(string taskId, string userId);
    IMyCollection<TaskItem> GetTasksForUser(string userId);
    // public IMyCollection<string> GetTasksForUser(string userId);

    // IMyCollection<TaskItem> GetTasksForUser(int userId);

    // IMyCollection<User> GetUsersForTask(int taskId);
}