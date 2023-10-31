using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;

namespace aiKart.Repositories;
public class UserDeckRepository : IUserDeckRepository
{
    private readonly DataContext dbContext;

    public UserDeckRepository(DataContext context)
    {
        dbContext = context;
    }

    public async Task<IEnumerable<Deck>> GetDecksByUserAsync(string username)
    {
        return await dbContext.UserDecks
            .Where(ud => ud.User.Name == username)
            .Select(ud => ud.Deck)
            .ToListAsync();
    }
}
