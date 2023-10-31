using aiKart.Models;

public interface IUserRepository
{
    Task<User?> GetUserByNameAsync(string username);
}