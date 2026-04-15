public interface ITaskUserRepository // Basel
{
    IMyCollection<TaskUser> Load(); //Basel
    void Save(IMyCollection<TaskUser> relations); // Basel
    IMyCollection<string> GetUsersForTask(IMyCollection<TaskUser> relations, string taskId); // Basel
    // public IMyCollection<string> GetTasksForUser(string userId); // jaro

    // void Save(IMyCollection<TaskUser> relations);

    IMyCollection<string> GetTasksForUser(IMyCollection<TaskUser> allRelations, string userId); // jaro
}