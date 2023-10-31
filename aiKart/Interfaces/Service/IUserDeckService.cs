using aiKart.Models;

namespace aiKart.Interfaces;

public interface IUserDeckService
{
    public Task<IEnumerable<Deck>> GetDecksByUserAsync(string username);
}
