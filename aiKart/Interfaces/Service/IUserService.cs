using aiKart.Models;

namespace aiKart.Interfaces;

public interface IUserService
{
    IEnumerable<User> GetUsers();
    User? GetUser(string name);
    User? GetUser(int id);
    bool UserExists(int id);
    bool UserExists(string name);
    ICollection<User> GetUsersOfADeck(int deckId);
    ICollection<Deck> GetDecksByUser(int userId);
    bool AddUser(User user);
}

