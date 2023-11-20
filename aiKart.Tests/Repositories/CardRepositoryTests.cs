using Xunit;
using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;
using aiKart.Repositories;
using aiKart.States;
using System.Linq;

namespace aiKart.Tests
{
    public class CardRepositoryTests
    {
        private DataContext _context;
        private CardRepository _cardRepository;

        public CardRepositoryTests()
        {
            string uniqueDatabaseName = Guid.NewGuid().ToString();
            ResetDatabase(uniqueDatabaseName);
        }

        private void ResetDatabase(string databaseName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            _context = new DataContext(options);
            _context.Cards.Add(new Card { Id = 1, State = CardState.Unanswered, DeckId = 1 });
            _context.Cards.Add(new Card { Id = 2, State = CardState.Answered, DeckId = 2 });
            _context.Cards.Add(new Card { Id = 3, State = CardState.PartiallyAnswered, DeckId = 3 });
            _context.SaveChanges();

            _cardRepository = new CardRepository(_context);
        }

        [Fact]
        public void GetCardById_CardExists_ReturnsCard()
        {
            var card = _cardRepository.GetCardById(1);
            Assert.NotNull(card);
            Assert.Equal(CardState.Unanswered, card.State);
        }

        [Fact]
        public void GetCardById_CardDoesNotExist_ReturnsNull()
        {
            var card = _cardRepository.GetCardById(999);
            Assert.Null(card);
        }

        [Fact]
        public void CardExists_CardExists_ReturnsTrue()
        {
            var result = _cardRepository.CardExists(1);
            Assert.True(result);
        }

        [Fact]
        public void CardExists_CardDoesNotExist_ReturnsFalse()
        {
            var result = _cardRepository.CardExists(999);
            Assert.False(result);
        }

        [Fact]
        public void AddCardToDeck_CardIsValid_ReturnsTrue()
        {
            var cardToAdd = new Card { Id = 4, State = CardState.Answered, DeckId = 4 };
            var result = _cardRepository.AddCardToDeck(cardToAdd);
            Assert.True(result);
        }

        [Fact]
        public void DeleteCard_CardIsValid_ReturnsTrue()
        {
            var cardToDelete = _context.Cards.Find(1);
            var result = _cardRepository.DeleteCard(cardToDelete);
            Assert.True(result);
        }

        [Fact]
        public void UpdateCardState_CardIsValid_ReturnsTrue()
        {
            var cardToUpdate = new Card { Id = 1, State = CardState.Answered };
            var result = _cardRepository.UpdateCardState(cardToUpdate);
            Assert.True(result);
        }

        [Fact]
        public void UpdateCard_CardIsValid_ReturnsTrue()
        {
            var cardToUpdate = _context.Cards.Find(1);
            var result = _cardRepository.UpdateCard(cardToUpdate);
            Assert.True(result);
        }
    }
}
