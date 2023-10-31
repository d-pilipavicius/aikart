using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;

namespace aiKart.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByNameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Name == username);
    }
}
