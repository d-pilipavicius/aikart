using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;

namespace aiKart.Repositories;

public class DeckRepository : IDeckRepository
{
    private readonly DataContext context;

    public DeckRepository(DataContext context)
    {
        this.context = context;
    }
 
    public ICollection<Deck> GetDecks()
    {
        return context.Decks.OrderBy(d => d.Id).ToList();      // later this could be modified to order be by last edit
    }

    public Deck GetDeck(int id)
    {
        return context.Decks.Find(id);
    }

    public ICollection<Card> GetDeckCards(int deckId)
    {
        return context.Cards
            .Where(c => c.DeckId == deckId).ToList();
    }

    public bool AddDeck(Deck deck)
    {
        context.Decks.Add(deck);
        return Save();
    }

    public bool DeleteDeck(Deck deck)
    {
        context.Decks.Remove(deck);
        return Save();
    }

    public bool UpdateDeck(Deck deck)
    {
        context.Decks.Update(deck);
        return Save();
    }

    public bool Save()
    {
        var saved = context.SaveChanges();
        return saved > 0 ? true : false;
    }
    
}