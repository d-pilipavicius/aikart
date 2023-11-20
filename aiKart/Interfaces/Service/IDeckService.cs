using aiKart.Models;
using aiKart.Services;

namespace aiKart.Interfaces;

public interface IDeckService
{
    IEnumerable<Deck> GetAllDecks();
    Deck? GetDeckById(int id);
    bool DeckExistsById(int id);
    bool DeckExistsByName(string name);
    bool AddDeck(Deck deck);
    bool DeleteDeck(Deck deck);
    bool UpdateDeck(Deck deck);
    Task<Deck> GetDeckByIdAsync(int deckId);
    IEnumerable<Card> GetCardsInDeck(int deckId);
    IEnumerable<Card> GetShuffledDeckCards(int deckId);
    IEnumerable<Deck> GetAllDecksIncludingCards();

    event DeckService.DeckChangeHandler DeckCreated;
    event DeckService.DeckChangeHandler DeckUpdated;
}

