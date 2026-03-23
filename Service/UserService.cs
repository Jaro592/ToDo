public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    public void AddUser(string name)
    {
        var users = _userRepository.LoadUsers();
        int newId = GenerateId(users);
        var user = new User(name);
        user.UserID = newId;
        users.Add(user);
        _userRepository.SaveUsers(users);
    }

    public User? FindUser(string name)
    {
        var users = _userRepository.LoadUsers();
        return users.FindBy(name, (k, key) => k.Name.CompareTo(key));

    }


    public IMyCollection<User> GetAllUsers()
    {
        return _userRepository.LoadUsers();
    }

    private int GenerateId(IMyCollection<User> users)
    {
        int maxId = 0;
        var iterator = users.GetIterator();
        while (iterator.HasNext())
        {
            var user = iterator.Next();
            if (user.UserID > maxId)
            {
                maxId = user.UserID;
            }
        }
        return maxId += 1;
    }
}