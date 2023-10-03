using System.ComponentModel.DataAnnotations;
using aiKart.States;

namespace aiKart.Models;
public class Card
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int DeckId { get; set; }
    public Deck Deck { get; set; }
    
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
    public CardState State { get; set; } = CardState.Unanswered;
}
