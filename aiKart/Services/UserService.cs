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

    public async Task<User?> GetUserByNameAsync(string username)
    {
        return await _userRepository.GetUserByNameAsync(username);
    }
}