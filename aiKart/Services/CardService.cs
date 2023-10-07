using aiKart.Interfaces;
using aiKart.Models;
using System.Collections.Generic;

namespace aiKart.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Card GetCardById(int id)
        {
            return _cardRepository.GetCardById(id);
        }

        public bool CardExists(int id)
        {
            return _cardRepository.CardExists(id);
        }

        public bool AddCardToDeck(Card card)
        {
            return _cardRepository.AddCardToDeck(card);
        }

        public bool DeleteCard(Card card)
        {
            if (card == null) return false;
            return _cardRepository.DeleteCard(card);
        }

        public bool UpdateCard(Card card)
        {
            return _cardRepository.UpdateCard(card);
        }
    }
}