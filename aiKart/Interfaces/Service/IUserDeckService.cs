using aiKart.Models;
using System.Collections.Generic;

namespace aiKart.Interfaces
{
    public interface IUserDeckService
    {
        ICollection<UserDeck> GetUserDecks();
        ICollection<Deck?> GetDecksByUser(int userId);
        ICollection<User?> GetUsersOfADeck(int deckId);
        bool UserDeckExists(int userId, int deckId);
        bool AddUserDeck(UserDeck userDeck);
        bool DeleteUserDeck(int userId, int deckId);
        bool Save();
    }
}
