namespace aiKart.Models;
public class Deck
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? CreatorName { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastEditDate { get; set; } = DateTime.UtcNow;
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
