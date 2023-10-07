using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;

namespace aiKart.Repositories;

public class DeckRepository : IDeckRepository
{
    private readonly DataContext _context;

    public DeckRepository(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<Deck> GetDecks()
    {
        return _context.Decks.OrderBy(d => d.Id).ToList();
    }

    public bool DeckExistsById(int id)
    {
        return _context.Decks.Any(d => d.Id == id);
    }

    public bool DeckExistsByName(string name)
    {
        return _context.Decks.Any(d => string.Equals(d.Name, name));
    }

    public Deck GetDeck(int id)
    {
        return _context.Decks.FirstOrDefault(d => d.Id == id);
    }

    public IEnumerable<Card> GetDeckCards(int deckId)
    {
        return _context.Cards
            .Where(c => c.DeckId == deckId).ToList();
    }

    public bool AddDeck(Deck deck)
    {
        deck.CreationDate = DateTime.UtcNow;
        deck.LastEditDate = DateTime.UtcNow;
        _context.Decks.Add(deck);
        return Save();
    }

    public bool DeleteDeck(Deck deck)
    {
        _context.Decks.Remove(deck);
        return Save();
    }

    public bool UpdateDeck(Deck deck)
    {
        deck.LastEditDate = DateTime.UtcNow;
        _context.Decks.Update(deck);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

}