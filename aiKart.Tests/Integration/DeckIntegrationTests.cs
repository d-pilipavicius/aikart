using Xunit;
using aiKart.Data;
using aiKart.Models;
using aiKart.Repositories;
using aiKart.Services;
using aiKart.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace aiKart.Tests.Integration
{
    public class DeckIntegrationTests
    {
        private readonly DeckService _deckService;
        private readonly DataContext _context;

        public DeckIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            _context = new DataContext(options);
            var deckRepository = new DeckRepository(_context);
            var cardRepository = new CardRepository(_context); 
            var userDeckService = new UserDeckService(new UserDeckRepository(_context)); 
            var cardShuffler = new Shuffler<Card>(); 

            _deckService = new DeckService(deckRepository, cardShuffler, userDeckService, cardRepository);
        }

        [Fact]
        public async Task AddDeck_ShouldCreateNewDeckInDatabase()
        {
            // Arrange
            var newDeck = new Deck { Name = "Test Deck", Description = "A test deck" };

            // Act
            var result = _deckService.AddDeck(newDeck);

            // Assert
            Assert.True(result);
            var deckInDb = await _context.Decks.FirstOrDefaultAsync(d => d.Name == "Test Deck");
            Assert.NotNull(deckInDb);
            Assert.Equal("A test deck", deckInDb.Description);
        }

        [Fact]
        public async Task GetDeckByIdAsync_DeckExists_ReturnsDeck()
        {
            // Arrange
            var existingDeck = new Deck { Name = "Existing Deck", Description = "Existing Description" };
            _context.Decks.Add(existingDeck);
            await _context.SaveChangesAsync();

            // Act
            var deck = await _deckService.GetDeckByIdAsync(existingDeck.Id);

            // Assert
            Assert.NotNull(deck);
            Assert.Equal("Existing Deck", deck.Name);
        }

        [Fact]
        public async Task UpdateDeck_DeckExists_UpdatesDeck()
        {
            // Arrange
            var deckToUpdate = new Deck { Name = "Old Name", Description = "Old Description" };
            _context.Decks.Add(deckToUpdate);
            await _context.SaveChangesAsync();

            deckToUpdate.Name = "Updated Name";

            // Act
            var result = _deckService.UpdateDeck(deckToUpdate);

            // Assert
            Assert.True(result);
            var updatedDeck = await _context.Decks.FindAsync(deckToUpdate.Id);
            Assert.NotNull(updatedDeck);
            Assert.Equal("Updated Name", updatedDeck.Name);
        }

        // Additional tests for delete, shuffle, clone, etc. can be added here...

        // Dispose the context after tests
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
