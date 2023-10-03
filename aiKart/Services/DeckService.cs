using aiKart.Interfaces;
using aiKart.Models;
using System.Collections.Generic;

namespace aiKart.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        public ICollection<Deck> GetDecks()
        {
            return _deckRepository.GetDecks();
        }

        public Deck GetDeck(int id)
        {
            return _deckRepository.GetDeck(id);
        }

        public ICollection<Card> GetDeckCards(int deckId)
        {
            return _deckRepository.GetDeckCards(deckId);
        }

        public bool AddDeck(Deck deck)
        {
            return _deckRepository.AddDeck(deck);
        }

        public bool DeleteDeck(Deck deck)
        {
            return _deckRepository.DeleteDeck(deck);
        }

        public bool UpdateDeck(Deck deck)
        {
            return _deckRepository.UpdateDeck(deck);
        }
    }
}
