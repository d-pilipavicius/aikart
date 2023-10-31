using aiKart.Models;
public interface IUserDeckRepository
{
    Task<IEnumerable<Deck>> GetDecksByUserAsync(string username);
}