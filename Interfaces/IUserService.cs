public interface IUserService
{
    void AddUser(string name);
    User? FindUser(string name);
    bool DeleteUser(string name); //akif

    void SaveAll(); // jaro


    IMyCollection<User> GetAllUsers();
}