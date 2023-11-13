using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace aiKart.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        //  concurrent collection
        private ConcurrentDictionary<int, Deck> _deckCache = new ConcurrentDictionary<int, Deck>();

        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        public IEnumerable<Deck> GetAllDecksIncludingCards()
        {
            return _deckRepository.GetDecksIncludingCards();
        }

        public IEnumerable<Deck> GetAllDecks()
        {
            return _deckRepository.GetDecks();
        }
        public async Task<Deck> GetDeckByIdAsync(int deckId)
        {
            // Try to get the deck from cache first
            if (_deckCache.TryGetValue(deckId, out var cachedDeck))
            {
                return cachedDeck; // Return cached deck if it exists
            }

            // Fetch from the database if not found in cache
            var deck = await _deckRepository.GetDeckByIdAsync(deckId);
            if (deck != null)
            {
                // Add or update cache after fetching from the database
                _deckCache.AddOrUpdate(deckId, deck, (key, oldValue) => deck);
            }

            return deck;
        }

        public Deck? GetDeckById(int id)
        {
            return _deckRepository.GetDeck(id);
        }

        public bool DeckExistsById(int id)
        {
            return _deckRepository.DeckExistsById(id);
        }

        public bool DeckExistsByName(string name)
        {
            return _deckRepository.DeckExistsByName(name);
        }

        public bool AddDeck(Deck deck)
        {
            return _deckRepository.AddDeck(deck);
        }

        public bool DeleteDeck(Deck deck)
        {
            if (deck == null) return false;
            return _deckRepository.DeleteDeck(deck);
        }

        public bool UpdateDeck(Deck deck)
        {
            return _deckRepository.UpdateDeck(deck);
        }

        public IEnumerable<Card> GetCardsInDeck(int deckId)
        {
            return _deckRepository.GetDeckCards(deckId);
        }
    }

}
