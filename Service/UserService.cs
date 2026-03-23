public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    public void AddUser(string name)
    {
        var users = _userRepository.LoadUsers();
        Guid newId = GenerateGUID();
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

    private Guid GenerateGUID() // jaro
    {
        return Guid.NewGuid();
    }
}