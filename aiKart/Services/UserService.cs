using aiKart.Models;
using aiKart.Interfaces;

namespace aiKart.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    public User? GetUser(int id)
    {
        return _userRepository.GetUser(id);
    }

    public User? GetUser(string name)
    {
        return _userRepository.GetUser(name);
    }

    public bool UserExists(int id)
    {
        return _userRepository.UserExists(id);
    }

    public bool UserExists(string name)
    {
        return _userRepository.UserExists(name);
    }

    public bool AddUser(User user)
    {
        if (!_userRepository.UserExists(user.Name))
        {
            return _userRepository.AddUser(user);
        }
        else
        {
            return false;
        }
    }
}