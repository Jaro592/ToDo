public interface IUserService
{
    bool AddUser(string name);
    User? FindUser(string name);
    bool DeleteUser(string name); //akif

    void SaveAll(); // jaro


    IMyCollection<User> GetAllUsers();
    IMyCollection<User> GetUsersByIds(IMyCollection<string> userIds);
}