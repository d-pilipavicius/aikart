using aiKart.Models;

namespace aiKart.Interfaces
{
    public interface ICardService
    {
        Card? GetCardById(int id);
        bool CardExists(int id);
        bool AddCardToDeck(Card card);
        bool DeleteCard(Card card);
        bool UpdateCard(Card card);

        bool UpdateCardState(Card card);
    }
}