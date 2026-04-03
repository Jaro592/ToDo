public interface ITaskUserRepository // Basel
{
    IMyCollection<TaskUser> Load(); //Basel
    void Save(IMyCollection<TaskUser> relations); // Basel
    IMyCollection<string> GetUsersForTask(string taskId); // Basel
    // public IMyCollection<string> GetTasksForUser(string userId); // jaro

    // void Save(IMyCollection<TaskUser> relations);

    public IMyCollection<string> GetTasksForUser(IMyCollection<TaskUser> allRelations, string userId); // jaro
}