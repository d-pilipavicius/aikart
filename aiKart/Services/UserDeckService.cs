using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aiKart.Services
{
    public class UserDeckService : IUserDeckService
    {
        private readonly DataContext _context;

        public UserDeckService(DataContext context)
        {
            _context = context;
        }

        public ICollection<UserDeck> GetUserDecks()
        {
            return _context.UserDecks.OrderBy(u => u.UserId).ToList();
        }

        public ICollection<Deck?> GetDecksByUser(int userId)
        {
            return _context.UserDecks.Where(u => u.UserId == userId).Include(o => o.Deck.Cards).Select(o => o.Deck).ToList();
        }

        public ICollection<User?> GetUsersOfADeck(int deckId)
        {
            return _context.UserDecks.Where(u => u.DeckId == deckId).Select(o => o.User).ToList();
        }

        public bool UserDeckExists(int userId, int deckId)
        {
            return _context.UserDecks.Any(u => u.UserId == userId && u.DeckId == deckId);
        }

        public bool AddUserDeck(UserDeck userDeck)
        {
            _context.Add(userDeck);
            return Save();
        }

        public bool DeleteUserDeck(int userId, int deckId)
        {
            var userDeck = _context.UserDecks.FirstOrDefault(ud => ud.UserId == userId && ud.DeckId == deckId);
            if (userDeck == null) return false;
            _context.UserDecks.Remove(userDeck);
            return Save();
        }

        public bool Save()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (Console.WriteLine is not the best way to handle exceptions here)
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
                return false;
            }
        }
    }
}
