using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;
using aiKart.Utils;

namespace aiKart.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly Shuffler<Card> _cardShuffler;

        //  concurrent collection
        private ConcurrentDictionary<int, Deck> _deckCache = new ConcurrentDictionary<int, Deck>();

        // Define a delegate for post-deck creation or update actions
        public delegate void DeckChangeHandler(Deck deck);

        // Define events based on the delegate
        public event DeckChangeHandler DeckCreated;
        public event DeckChangeHandler DeckUpdated;

        public DeckService(IDeckRepository deckRepository, Shuffler<Card> cardShuffler)
        {
            _deckRepository = deckRepository;
            _cardShuffler = cardShuffler;
        }

        // Method to raise the DeckCreated event
        protected virtual void OnDeckCreated(Deck deck)
        {
            DeckCreated?.Invoke(deck);
        }

        // Method to raise the DeckUpdated event
        protected virtual void OnDeckUpdated(Deck deck)
        {
            DeckUpdated?.Invoke(deck);
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
            var result = _deckRepository.AddDeck(deck);
            if (result)
            {
                // Update the cache
                _deckCache.AddOrUpdate(deck.Id, deck, (id, oldDeck) => deck);

                // Raise the event
                OnDeckCreated(deck);
            }
            return result;
        }

        public bool DeleteDeck(Deck deck)
        {
            if (deck == null) return false;
            return _deckRepository.DeleteDeck(deck);
        }

        public bool UpdateDeck(Deck deck)
        {
            var result = _deckRepository.UpdateDeck(deck);
            if (result)
            {
                // Update the cache
                _deckCache.AddOrUpdate(deck.Id, deck, (id, oldDeck) => deck);

                // Raise the event
                OnDeckUpdated(deck);
            }
            return result;
        }

        public IEnumerable<Card> GetCardsInDeck(int deckId)
        {
            return _deckRepository.
            GetDeckCards(deckId);
        }

        public IEnumerable<Card> GetShuffledDeckCards(int deckId)
        {
            IEnumerable<Card> deckCards = _deckRepository.GetDeckCards(deckId);

            deckCards = _cardShuffler.Shuffle(deckCards);

            return deckCards;
        }
    }


}
