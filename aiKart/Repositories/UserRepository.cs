using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;
using aiKart.Interfaces;

namespace aiKart.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<User> GetUsers()
    {
        return _context.Users.OrderBy(user => user.Id).ToList();
    }

    public User? GetUser(int id)
    {
        return _context.Users.Find(id);
    }

    public User? GetUser(string name)
    {
        return _context.Users.Where(user => user.Name == name).FirstOrDefault();
    }

    public bool UserExists(int id)
    {
        return _context.Users.Any(user => user.Id == id);
    }

    public bool UserExists(string name)
    {
        return _context.Users.Any(user => user.Name == name);
    }

    public bool AddUser(User user)
    {
        _context.Add(user);
        return Save();
    }

    public bool Save()
    {
        try
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
            return false;
        }
    }
}
