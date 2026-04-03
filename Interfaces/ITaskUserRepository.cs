public interface ITaskUserRepository // Basel
{
    IMyCollection<TaskUser> Load(); //Basel
    void Save(IMyCollection<TaskUser> relations); // Basel
    IMyCollection<string> GetUsersForTask(string taskId); // Basel
    public IMyCollection<string> GetTasksForUser(string userId); // jaro
}