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
        string newId = NewSerializeString();
        var user = new User(name);
        user.UserID = newId;
        _users.Add(user);
    }

    public User? FindUser(string name)
    {

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


    }
    public IMyCollection<User> GetUsersByIds(IMyCollection<string> userIds) //Basel
    {
        IMyCollection<User> users = new MyLinkedList<User>();

        return userIds.Reduce(users, (list, id) =>
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                list.Add(user);
            }
            return list;
        });
    }

    public void SaveAll() // jaro
    {
        _userRepository.SaveUsers(_users);
    }
}