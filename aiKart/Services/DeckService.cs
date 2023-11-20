using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace aiKart.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private Shuffler<Card> _cardShuffler;

        public DeckService(IDeckRepository deckRepository, Shuffler<Card> cardShuffler)
        {
            _deckRepository = deckRepository;
            _cardShuffler = cardShuffler;
        }
        public IEnumerable<Deck> GetAllDecksIncludingCards()
        {
            return _deckRepository.GetDecksIncludingCards();
        }

        public IEnumerable<Deck> GetAllDecks()
        {
            return _deckRepository.GetDecks();
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

        public IEnumerable<Card> GetShuffledDeckCards(int deckId)
        {
            IEnumerable<Card> deckCards = _deckRepository.GetDeckCards(deckId);

            deckCards = _cardShuffler.Shuffle(deckCards);

            return deckCards;
        }
    }
}
