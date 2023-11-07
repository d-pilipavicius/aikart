using aiKart.Models;

namespace aiKart.Interfaces;
public interface IUserDeckRepository
{
    ICollection<UserDeck> GetUserDecks();
    ICollection<User> GetUsersOfADeck(int deckId);
    ICollection<Deck> GetDecksByUser(int userId);
    bool UserDeckExists(int userId, int deckId);
    bool AddUserDeck(UserDeck userDeck);
    bool DeleteUserDeck(UserDeck userDeck);
    bool Save();
}