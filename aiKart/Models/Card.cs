using aiKart.States;

namespace aiKart.Models;
public class Card
{
    public int Id { get; set; }
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
    public CardState State { get; set; } = CardState.Unanswered;
    public double EFactor { get; set; } = 2.5;
    public int IntervalInDays { get; set; } = 1;
    public DateTime? LastRepetition { get; set; } = null;

    public int DeckId { get; set; }
    public Deck? Deck { get; set; }
}
