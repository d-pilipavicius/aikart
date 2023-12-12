using aiKart.Interfaces;
using aiKart.Models;
using aiKart.States;
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

        public bool UpdateAnswerCount(int userId, int deckId, CardState state, int count)
        {
            var userDeck = _userDeckRepository.GetUserDeck(userId, deckId);

            switch (state)
            {
                case CardState.Answered:
                    userDeck.CorrectAnswers += count;
                    break;
                case CardState.PartiallyAnswered:
                    userDeck.PartiallyCorrectAnswers += count;
                    break;
                case CardState.Unanswered:
                    userDeck.IncorrectAnswers += count;
                    break;
                default:
                    return false;
            }

            return _userDeckRepository.UpdateUserDeck(userDeck);
        }

        public UserDeck GetUserDeckStatistics(int userId, int deckId)
        {
            var userDeck = _userDeckRepository.GetUserDeck(userId, deckId);

            return userDeck;
        }

        public bool IncrementDeckSolves(int userId, int deckId, int count)
        {
            var userDeck = _userDeckRepository.GetUserDeck(userId, deckId);
            userDeck.TimesSolved += count;

            return _userDeckRepository.UpdateUserDeck(userDeck);
        }

        public bool Save()
        {
            return _userDeckRepository.Save();
        }
    }
}
