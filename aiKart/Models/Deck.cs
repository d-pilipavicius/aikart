using aiKart.Interfaces;

namespace aiKart.Models;

public class Deck : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int CreatorId { get; set; }
    public string? Description { get; set; }
    public string? CreatorName { get; set; }
    public bool IsPublic { get; set; } = false;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastEditDate { get; set; } = DateTime.UtcNow;

    public ICollection<Card> Cards { get; set; } = new List<Card>();
    public ICollection<UserDeck>? UserDecks { get; set; }
}
