using aiKart.Interfaces;
using aiKart.Models;
using aiKart.States;
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

        public Card? GetCardById(int id)
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

        public bool UpdateCardState(Card card)
        {
            return _cardRepository.UpdateCardState(card);
        }

        public bool UpdateCardRepetitionInterval(int cardId, CardState state)
        {
            Card card = _cardRepository.GetCardById(cardId);

            if (card == null) return false;

            if (card.LastRepetition != null)
            {
                int stateValue = (int)state;
                double EFPrime = card.EFactor + (0.1 - (5 - stateValue) * (0.08 + (5 - stateValue) * 0.02));
                card.EFactor = Math.Max(EFPrime, 1.3);

                if (stateValue < 3)
                {
                    card.IntervalInDays = 1;
                }
                else
                {
                    card.IntervalInDays *= (int)Math.Round(EFPrime);
                }
            }

            card.LastRepetition = DateTime.Now;

            return _cardRepository.UpdateCard(card);
        }

    }
}