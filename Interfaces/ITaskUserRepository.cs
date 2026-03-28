public interface ITaskUserRepository
{
    IMyCollection<TaskUser> Load();
    void Save(IMyCollection<TaskUser> relations);

    public IMyCollection<string> GetTasksForUser(string userId); // jaro
}