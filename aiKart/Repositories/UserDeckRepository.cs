using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;
using aiKart.Interfaces;

namespace aiKart.Repositories;

public class UserDeckRepository : IUserDeckRepository
{
    private readonly DataContext _context;

    public UserDeckRepository(DataContext context)
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
        return _context.UserDecks.Any(u => (u.UserId == userId) && (u.DeckId == deckId));
    }

    public bool AddUserDeck(UserDeck userDeck)
    {
        _context.Add(userDeck);
        return Save();
    }

    public bool DeleteUserDeck(UserDeck userDeck)
    {
        throw new NotImplementedException();
    }

    public bool Save()
    {
        try
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
            return false;
        }
    }
}
