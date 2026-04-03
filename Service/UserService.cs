using System.Reflection.Metadata.Ecma335;

public class UserService : Serialize, IUserService
{

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    public void AddUser(string name)
    {
        var users = _userRepository.LoadUsers();
        string newId = NewSerializeString();
        var user = new User(name);
        user.UserID = newId;
        users.Add(user);
        _userRepository.SaveUsers(users);
    }

    public User? FindUser(string name)
    {
        var users = GetAllUsers(); // changed from var users = _userRepository.LoadUsers(); to use the already existing function (GetAllUsers) that does the same -akif
        return users.FindBy(name, (k, key) => k.Name.CompareTo(key));

    }

    public IMyCollection<User> GetAllUsers()
    {
        return _userRepository.LoadUsers();
    }

    public bool DeleteUser(string name) //akif
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var user = FindUser(name);
        if (user == null)
            return false;

        var users = GetAllUsers();
        users.Remove(user);
        _userRepository.SaveUsers(users);
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
    // private Guid GenerateGUID() // jaro
    // {
    //     return Guid.NewGuid();
    // }
}