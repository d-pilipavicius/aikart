using aiKart.Models;
using aiKart.Interfaces;

namespace aiKart.Services;

public class UserDeckService : IUserDeckService
{
    private readonly IUserDeckRepository _userDeckRepository;

    public UserDeckService(IUserDeckRepository userDeckRepository)
    {
        _userDeckRepository = userDeckRepository;
    }

    public async Task<IEnumerable<Deck>> GetDecksByUserAsync(string username)
    {
        return await _userDeckRepository.GetDecksByUserAsync(username);
    }
}