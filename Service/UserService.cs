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

    public bool AddUser(string name)
    {
        if (FindUser(name) != null)
        {
            return false;
        }
        string newId = NewSerializeString();
        var user = new User(newId, name);

        _users.Add(user);

        return true;
    }

    public User? FindUser(string name)
    {
        name = name.Trim();

        return _users.FindBy(name, (k, key) => string.Compare(k.Name, key, StringComparison.OrdinalIgnoreCase)); // jaro use my own storage instead of loading from json every time,  changes by Basel

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