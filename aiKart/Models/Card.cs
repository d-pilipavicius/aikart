using aiKart.States;

namespace aiKart.Models;
public class Card
{
    public int Id { get; set; }
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
    public CardState State { get; set; } = CardState.Unanswered;

    public int DeckId { get; set; }
    public Deck? Deck { get; set; }
}
