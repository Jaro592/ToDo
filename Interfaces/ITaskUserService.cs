public interface ITaskUserService
{
    void Assign(string taskId, string userId); // Basel
    IMyCollection<TaskItem> GetTasksForUser(string userId);
    // public IMyCollection<string> GetTasksForUser(string userId);

    // IMyCollection<TaskItem> GetTasksForUser(int userId);

    IMyCollection<string> GetUsersForTask(string taskId);
}