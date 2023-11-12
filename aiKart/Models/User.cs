namespace aiKart.Models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set;}
    
    public ICollection<UserDeck>? UserDecks { get; set; }

}