using aiKart.Models;
using aiKart.States;
using Microsoft.AspNetCore.Mvc;
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
        bool UpdateAnswerCount(int userId, int deckId, CardState state, int count);
        UserDeck GetUserDeckStatistics(int userId, int deckId);
        bool IncrementDeckSolves(int userId, int deckId, int count);
    }
}
