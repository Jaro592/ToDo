public interface ITaskUserRepository
{
    IMyCollection<TaskUser> Load();
    void Save(IMyCollection<TaskUser> relations);
}