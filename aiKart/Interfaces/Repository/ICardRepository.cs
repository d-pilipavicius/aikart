using aiKart.Dtos;
using aiKart.Models;

namespace aiKart.Interfaces;

public interface ICardRepository
{
    Card GetCard(int id);
    bool CardExists(int id);
    bool AddCardToDeck(int deckId, string question, string answer);
    bool DeleteCard(Card card);
    bool UpdateCard(Card card);
    bool Save();
}