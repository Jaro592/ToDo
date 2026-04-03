public interface IUserRepository
{
    IMyCollection<User> LoadUsers();
    void SaveUsers(IMyCollection<User> users);
    User GetById(string id);// Basel
}