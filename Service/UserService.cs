using System.Reflection.Metadata.Ecma335;

public class UserService : Serialize, IUserService
{

    private readonly IUserRepository _userRepository;
    private IMyCollection<User> _users;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _users = _userRepository.LoadUsers(); // jaro load users once and keep in memory to avoid multiple file reads
    }

    public void AddUser(string name)
    {
        //var users = _userRepository.LoadUsers();
        string newId = NewSerializeString();
        var user = new User(name);
        user.UserID = newId;
        //users.Add(user);
        _users.Add(user);
        //_userRepository.SaveUsers(users);
    }

    public User? FindUser(string name)
    {
        // var users = GetAllUsers(); // changed from var users = _userRepository.LoadUsers(); to use the already existing function (GetAllUsers) that does the same -akif
        // return users.FindBy(name, (k, key) => k.Name.CompareTo(key));
        return _users.FindBy(name, (k, key) => k.Name.CompareTo(key)); // jaro use my own storage instead of loading from json every time

    }

    public IMyCollection<User> GetAllUsers()
    {
        return _users; // jaro use my own storage instead of loading from json every time
    }

    public bool DeleteUser(string name) //akif
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var user = FindUser(name);
        if (user == null)
            return false;
        _users.Remove(user);
        return true;

        //var users = GetAllUsers(); not needed anymore Jaro
        //users.Remove(user);
        //_userRepository.SaveUsers(users);
    }
    // private Guid GenerateGUID() // jaro
    // {
    //     return Guid.NewGuid();
    // }

    public void SaveAll() // jaro
    {
        _userRepository.SaveUsers(_users);
    }
}