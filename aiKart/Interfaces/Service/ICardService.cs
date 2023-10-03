using aiKart.Models;
using System.Collections.Generic;

namespace aiKart.Interfaces
{
    public interface ICardService
    {
        Card GetCard(int id);
        bool AddCardToDeck(int deckId, string question, string answer);
        bool DeleteCard(Card card);
        bool UpdateCard(Card card);
    }
}
