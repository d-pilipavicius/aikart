using System.Net.Quic;
using aiKart.Data;
using aiKart.Dtos;
using aiKart.Interfaces;
using aiKart.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace aiKart.Repositories;

public class CardRepository : ICardRepository
{
    private readonly DataContext context;

    public CardRepository(DataContext context)
    {
        this.context = context;
    }

    public Card GetCard(int id)
    {
        return context.Cards.Find(id);
    }

    public bool CardExists(int id)
    {
        return context.Cards.Any(c => c.Id == id);
    }

    public bool AddCardToDeck(int deckId, string question, string answer)
    {
        var deck = context.Decks.Find(deckId);

        Card card = new Card()
        {
            DeckId = deckId,
            Deck = deck,
            Question = question,
            Answer = answer
        };

        context.Add(card);

        return Save();
    }

    public bool DeleteCard(Card card)
    {
        context.Cards.Remove(card);
        return Save();
    }

    public bool UpdateCard(Card card)
    {
        context.Cards.Update(card);
        return Save();
    }

    public bool Save()
    {
        var saved = context.SaveChanges();
        return saved > 0 ? true : false;
    }

}