using Xunit;
using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;
using aiKart.Repositories;
using System;
using System.Linq;

namespace aiKart.Tests
{
    public class DeckRepositoryTests
    {
        private readonly DataContext _context;
        private readonly DeckRepository _deckRepository;

        public DeckRepositoryTests()
        {
            string uniqueDatabaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: uniqueDatabaseName)
                .Options;

            _context = new DataContext(options);

            _context.Decks.Add(new Deck { Id = 1, Name = "Science" });
            _context.Decks.Add(new Deck { Id = 2, Name = "Math" });
            _context.SaveChanges();

            _deckRepository = new DeckRepository(_context);
        }

        [Fact]
        public void GetDecks_ReturnsAllDecks()
        {
            var result = _deckRepository.GetDecks();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetDeckById_DeckExists_ReturnsDeck()
        {
            var result = _deckRepository.GetDeck(1);
            Assert.NotNull(result);
            Assert.Equal("Science", result.Name);
        }

        [Fact]
        public void GetDeckById_DeckDoesNotExist_ReturnsNull()
        {
            var result = _deckRepository.GetDeck(999);
            Assert.Null(result);
        }

        [Fact]
        public void AddDeck_ValidDeck_ReturnsTrue()
        {
            var newDeck = new Deck { Id = 3, Name = "Geography" };
            var result = _deckRepository.AddDeck(newDeck);
            Assert.True(result);
        }

        [Fact]
        public void DeleteDeck_ValidDeck_ReturnsTrue()
        {
            var deckToDelete = _context.Decks.Find(1);
            var result = _deckRepository.DeleteDeck(deckToDelete);
            Assert.True(result);
        }

        [Fact]
        public void UpdateDeck_ValidDeck_ReturnsTrue()
        {
            var deckToUpdate = _context.Decks.Find(1);
            deckToUpdate.Name = "UpdatedScience";
            var result = _deckRepository.UpdateDeck(deckToUpdate);
            Assert.True(result);
        }
    }
}
