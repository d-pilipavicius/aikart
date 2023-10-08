using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace aiKart.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        private readonly DataContext _dbContext;

        public DeckService(IDeckRepository deckRepository, DataContext dbContext)
        {
            _deckRepository = deckRepository;
            _dbContext = dbContext;
        }


        public IEnumerable<Deck> GetAllDecksIncludingCards()
        {
            return _dbContext.Decks.Include(d => d.Cards).ToList();
        }

        public IEnumerable<Deck> GetAllDecks()
        {
            return _deckRepository.GetDecks();
        }

        public Deck GetDeckById(int id)
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
