namespace aiKart.Models;

public class UserDeck
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int DeckId { get; set; }
    public Deck? Deck { get; set; }

    public int TimesSolved { get; set; } = 0;
    public int CorrectAnswers { get; set; } = 0;
    public int PartiallyCorrectAnswers { get; set; } = 0;
    public int IncorrectAnswers { get; set; } = 0;

}