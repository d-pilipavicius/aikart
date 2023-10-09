using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;

namespace aiKart.Repositories;

public class CardRepository : ICardRepository
{
    private readonly DataContext _context;

    public CardRepository(DataContext context)
    {
        _context = context;
    }

    public Card GetCardById(int id)
    {
        return _context.Cards.Find(id);
    }

    public bool CardExists(int id)
    {
        return _context.Cards.Any(c => c.Id == id);
    }

    public bool AddCardToDeck(Card card)
    {
        var deck = _context.Decks.Find(card.DeckId);
        card.Deck = deck;

        _context.Add(card);

        return Save();
    }

    public bool DeleteCard(Card card)
    {
        _context.Cards.Remove(card);
        return Save();
    }

    public bool UpdateCard(Card card)
    {
        _context.Cards.Update(card);
        return Save();
    }

    public bool Save()
    {
        try
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
            return false;
        }
    }


}