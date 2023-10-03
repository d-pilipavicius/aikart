using aiKart.Models;
using System.Collections.Generic;

namespace aiKart.Interfaces
{
    public interface IDeckService
    {
        ICollection<Deck> GetDecks();
        Deck GetDeck(int id);
        ICollection<Card> GetDeckCards(int deckId);
        bool AddDeck(Deck deck);
        bool DeleteDeck(Deck deck);
        bool UpdateDeck(Deck deck);
    }
}
