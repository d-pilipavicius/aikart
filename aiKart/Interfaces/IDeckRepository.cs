using aiKart.Dtos;
using aiKart.Models;

namespace aiKart.Interfaces;

public interface IDeckRepository
{
    ICollection<Deck> GetDecks();
    ICollection<Card> GetDeckCards(int deckId);
    Deck GetDeck(int id);
    bool AddDeck(Deck deck);
    bool DeleteDeck(Deck deck);
    bool UpdateDeck(Deck deck);
    bool Save();
}