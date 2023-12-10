using Xunit;
using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;
using aiKart.Repositories;
using System;
using System.Linq;

namespace aiKart.Tests
{
    public class UserDeckRepositoryTests
    {
        private readonly DataContext _context;
        private readonly UserDeckRepository _userDeckRepository;

        public UserDeckRepositoryTests()
        {
            string uniqueDatabaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: uniqueDatabaseName)
                .Options;

            _context = new DataContext(options);

            // Seed with some UserDeck data
            _context.UserDecks.AddRange(
                new UserDeck { UserId = 1, DeckId = 1 },
                new UserDeck { UserId = 2, DeckId = 1 },
                new UserDeck { UserId = 3, DeckId = 2 }
            );

            _context.Users.Add(new User { Id = 1, Name = "Alice" });
            _context.Users.Add(new User { Id = 2, Name = "Bob" });
            _context.Decks.Add(new Deck { Id = 1, Name = "Science" });
            _context.Decks.Add(new Deck { Id = 2, Name = "Math" });
            
            _context.SaveChanges();

            _userDeckRepository = new UserDeckRepository(_context);
        }

        [Fact]
        public void GetUserDecks_ShouldReturnAllUserDecks()
        {
            var result = _userDeckRepository.GetUserDecks();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetDecksByUser_ShouldReturnDecksForSpecificUser()
        {
            var result = _userDeckRepository.GetDecksByUser(1);
            Assert.Equal(1, result.Count); // Assuming user with ID 1 has 1 deck
        }

        [Fact]
        public void GetUsersOfADeck_ShouldReturnUsersForSpecificDeck()
        {
            var result = _userDeckRepository.GetUsersOfADeck(1);
            Assert.Equal(2, result.Count); // Assuming deck with ID 1 has 2 users
        }

        [Fact]
        public void UserDeckExists_ShouldReturnTrueIfExists()
        {
            bool exists = _userDeckRepository.UserDeckExists(1, 1);
            Assert.True(exists);
        }

        [Fact]
        public void AddUserDeck_ShouldAddUserDeck()
        {
            var userDeck = new UserDeck { UserId = 4, DeckId = 3 };
            bool result = _userDeckRepository.AddUserDeck(userDeck);

            Assert.True(result);
            Assert.Equal(4, _context.UserDecks.Count());
        }

        [Fact]
        public void DeleteUserDeck_ShouldDeleteUserDeck()
        {
            var userDeck = _context.UserDecks.First();
            bool result = _userDeckRepository.DeleteUserDeck(userDeck);

            Assert.True(result);
            Assert.Equal(2, _context.UserDecks.Count());
        }
    }
}
