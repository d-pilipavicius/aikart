using aiKart.Interfaces;
using aiKart.Models;
using System.Collections.Generic;

namespace aiKart.Services
{
    public class UserDeckService : IUserDeckService
    {
        private readonly IUserDeckRepository _userDeckRepository;

        public UserDeckService(IUserDeckRepository userDeckRepository)
        {
            _userDeckRepository = userDeckRepository;
        }

        public ICollection<UserDeck> GetUserDecks()
        {
            return _userDeckRepository.GetUserDecks();
        }

        public ICollection<Deck?> GetDecksByUser(int userId)
        {
            return _userDeckRepository.GetDecksByUser(userId);
        }

        public ICollection<User?> GetUsersOfADeck(int deckId)
        {
            return _userDeckRepository.GetUsersOfADeck(deckId);
        }

        public bool UserDeckExists(int userId, int deckId)
        {
            return _userDeckRepository.UserDeckExists(userId, deckId);
        }

        public bool AddUserDeck(UserDeck userDeck)
        {
            return _userDeckRepository.AddUserDeck(userDeck);
        }

        public bool DeleteUserDeck(int userId, int deckId)
        {
            var userDeck = _userDeckRepository.GetUserDecks().FirstOrDefault(ud => ud.UserId == userId && ud.DeckId == deckId);
            if (userDeck == null) return false;
            return _userDeckRepository.DeleteUserDeck(userDeck);
        }

        public bool Save()
        {
            return _userDeckRepository.Save();
        }
    }
}
