public interface IUserService
{
    void AddUser(string name);
    User? FindUser(string name);
    bool DeleteUser(string name); //akif

    IMyCollection<User> GetAllUsers();
    IMyCollection<User> GetUsersByIds(IMyCollection<string> userIds);
}