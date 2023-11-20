using aiKart.Models;

namespace aiKart.Interfaces;
public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUser(string name);
    User? GetUser(int id);
    bool UserExists(int id);
    bool UserExists(string name);
    bool AddUser(User user);
    bool Save();
}