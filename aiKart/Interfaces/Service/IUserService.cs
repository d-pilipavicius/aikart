using aiKart.Models;

namespace aiKart.Interfaces;

public interface IUserService
{
    public Task<User?> GetUserByNameAsync(string username);
}

