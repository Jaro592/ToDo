public interface IUserService
{
    void AddUser(string name);
    User? FindUser(string name);
    IMyCollection<User> GetAllUsers();
}