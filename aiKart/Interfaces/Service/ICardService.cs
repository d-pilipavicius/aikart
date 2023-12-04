using aiKart.Models;
using aiKart.States;

namespace aiKart.Interfaces;

public interface ICardService
{
    Card? GetCardById(int id);
    bool CardExists(int id);
    bool AddCardToDeck(Card card);
    bool DeleteCard(Card card);
    bool UpdateCard(Card card);
    bool UpdateCardRepetitionInterval(int cardId, CardState state);
    bool UpdateCardState(Card card);
}
